// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.WebSockets
{
    public abstract class WebSocket : IDisposable
    {
        public abstract WebSocketCloseStatus? CloseStatus { get; }
        public abstract string? CloseStatusDescription { get; }
        public abstract string? SubProtocol { get; }
        public abstract WebSocketState State { get; }

        public abstract void Abort();
        public abstract Task CloseAsync(WebSocketCloseStatus closeStatus,
            string? statusDescription,
            CancellationToken cancellationToken);
        public abstract Task CloseOutputAsync(WebSocketCloseStatus closeStatus,
            string? statusDescription,
            CancellationToken cancellationToken);
        public abstract void Dispose();
        public abstract Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer,
            CancellationToken cancellationToken);
        public abstract Task SendAsync(ArraySegment<byte> buffer,
            WebSocketMessageType messageType,
            bool endOfMessage,
            CancellationToken cancellationToken);

        public virtual async ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken)
        {
            if (MemoryMarshal.TryGetArray(buffer, out ArraySegment<byte> arraySegment))
            {
                WebSocketReceiveResult r = await ReceiveAsync(arraySegment, cancellationToken).ConfigureAwait(false);
                return new ValueWebSocketReceiveResult(r.Count, r.MessageType, r.EndOfMessage);
            }

            byte[] array = ArrayPool<byte>.Shared.Rent(buffer.Length);
            try
            {
                WebSocketReceiveResult r = await ReceiveAsync(new ArraySegment<byte>(array, 0, buffer.Length), cancellationToken).ConfigureAwait(false);
                new Span<byte>(array, 0, r.Count).CopyTo(buffer.Span);
                return new ValueWebSocketReceiveResult(r.Count, r.MessageType, r.EndOfMessage);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(array);
            }
        }

        public virtual ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken) =>
            MemoryMarshal.TryGetArray(buffer, out ArraySegment<byte> arraySegment) ?
                new ValueTask(SendAsync(arraySegment, messageType, endOfMessage, cancellationToken)) :
                SendWithArrayPoolAsync(buffer, messageType, endOfMessage, cancellationToken);

        public virtual ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, WebSocketMessageFlags messageFlags, CancellationToken cancellationToken = default)
        {
            return SendAsync(buffer, messageType, messageFlags.HasFlag(WebSocketMessageFlags.EndOfMessage), cancellationToken);
        }

        private async ValueTask SendWithArrayPoolAsync(
            ReadOnlyMemory<byte> buffer,
            WebSocketMessageType messageType,
            bool endOfMessage,
            CancellationToken cancellationToken)
        {
            byte[] array = ArrayPool<byte>.Shared.Rent(buffer.Length);
            try
            {
                buffer.Span.CopyTo(array);
                await SendAsync(new ArraySegment<byte>(array, 0, buffer.Length), messageType, endOfMessage, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(array);
            }
        }

        public static TimeSpan DefaultKeepAliveInterval
        {
            // In the .NET Framework, this pulls the value from a P/Invoke.  Here we just hardcode it to a reasonable default.
            get { return WebSocketDefaults.DefaultClientKeepAliveInterval; }
        }

        protected static void ThrowOnInvalidState(WebSocketState state, params WebSocketState[] validStates)
        {
            string validStatesText = string.Empty;

            if (validStates != null && validStates.Length > 0)
            {
                foreach (WebSocketState currentState in validStates)
                {
                    if (state == currentState)
                    {
                        return;
                    }
                }

                validStatesText = string.Join(", ", validStates);
            }

            throw new WebSocketException(WebSocketError.InvalidState, SR.Format(SR.net_WebSockets_InvalidState, state, validStatesText));
        }

        protected static bool IsStateTerminal(WebSocketState state) =>
            state == WebSocketState.Closed || state == WebSocketState.Aborted;

        public static ArraySegment<byte> CreateClientBuffer(int receiveBufferSize, int sendBufferSize)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(receiveBufferSize);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sendBufferSize);
            return new ArraySegment<byte>(new byte[Math.Max(receiveBufferSize, sendBufferSize)]);
        }

        public static ArraySegment<byte> CreateServerBuffer(int receiveBufferSize)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(receiveBufferSize);
            return new ArraySegment<byte>(new byte[receiveBufferSize]);
        }

        /// <summary>Creates a <see cref="WebSocket"/> that operates on a <see cref="Stream"/> representing a web socket connection.</summary>
        /// <param name="stream">The <see cref="Stream"/> for the connection.</param>
        /// <param name="isServer"><code>true</code> if this is the server-side of the connection; <code>false</code> if it's the client side.</param>
        /// <param name="subProtocol">The agreed upon sub-protocol that was used when creating the connection.</param>
        /// <param name="keepAliveInterval">The keep-alive interval to use, or <see cref="Timeout.InfiniteTimeSpan"/> to disable keep-alives.</param>
        /// <returns>The created <see cref="WebSocket"/>.</returns>
        public static WebSocket CreateFromStream(Stream stream, bool isServer, string? subProtocol, TimeSpan keepAliveInterval)
        {
            ArgumentNullException.ThrowIfNull(stream);

            if (!stream.CanRead || !stream.CanWrite)
            {
                throw new ArgumentException(!stream.CanRead ? SR.NotReadableStream : SR.NotWriteableStream, nameof(stream));
            }

            if (subProtocol != null)
            {
                WebSocketValidate.ValidateSubprotocol(subProtocol);
            }

            if (keepAliveInterval != Timeout.InfiniteTimeSpan && keepAliveInterval < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(keepAliveInterval), keepAliveInterval,
                    SR.Format(SR.net_WebSockets_ArgumentOutOfRange_TooSmall,
                    0));
            }

            return new ManagedWebSocket(stream, isServer, subProtocol, keepAliveInterval, WebSocketDefaults.DefaultKeepAliveTimeout);
        }

        /// <summary>Creates a <see cref="WebSocket"/> that operates on a <see cref="Stream"/> representing a web socket connection.</summary>
        /// <param name="stream">The <see cref="Stream"/> for the connection.</param>
        /// <param name="options">The options with which the websocket must be created.</param>
        public static WebSocket CreateFromStream(Stream stream, WebSocketCreationOptions options)
        {
            ArgumentNullException.ThrowIfNull(stream);
            ArgumentNullException.ThrowIfNull(options);

            if (!stream.CanRead || !stream.CanWrite)
                throw new ArgumentException(!stream.CanRead ? SR.NotReadableStream : SR.NotWriteableStream, nameof(stream));

            return new ManagedWebSocket(stream, options);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.")]
        public static bool IsApplicationTargeting45() => true;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.")]
        public static void RegisterPrefixes()
        {
            // The current WebRequest implementation in the libraries does not support upgrading
            // web socket connections.  For now, we throw.
            throw new PlatformNotSupportedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static WebSocket CreateClientWebSocket(Stream innerStream,
            string? subProtocol, int receiveBufferSize, int sendBufferSize,
            TimeSpan keepAliveInterval, bool useZeroMaskingKey, ArraySegment<byte> internalBuffer)
        {
            ArgumentNullException.ThrowIfNull(innerStream);

            if (!innerStream.CanRead || !innerStream.CanWrite)
            {
                throw new ArgumentException(!innerStream.CanRead ? SR.NotReadableStream : SR.NotWriteableStream, nameof(innerStream));
            }

            if (subProtocol != null)
            {
                WebSocketValidate.ValidateSubprotocol(subProtocol);
            }

            if (keepAliveInterval != Timeout.InfiniteTimeSpan && keepAliveInterval < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(keepAliveInterval), keepAliveInterval,
                    SR.Format(SR.net_WebSockets_ArgumentOutOfRange_TooSmall,
                    0));
            }

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(receiveBufferSize);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sendBufferSize);

            // Ignore useZeroMaskingKey. ManagedWebSocket doesn't currently support that debugging option.
            // Ignore internalBuffer. ManagedWebSocket uses its own small buffer for headers/control messages.
            return new ManagedWebSocket(innerStream, false, subProtocol, keepAliveInterval, WebSocketDefaults.DefaultKeepAliveTimeout);
        }
    }
}
