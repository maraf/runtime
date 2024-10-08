// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Tests Pinned handle for array of Objects...
// Pinning of "Object" type is not allowed and should throw an exception.

using System;
using System.Runtime.InteropServices;
using Xunit;

public class Test_PinObj_neg
{
    [Fact]
    public static int TestEntryPoint()
    {
        Object[] array = new Object[25];

        Console.WriteLine("Trying to pin array of objects..");
        Console.WriteLine("Should throw an exception");
        try
        {
            GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Expected ArgumentException");

            Console.WriteLine("Test passed!");
            return 100;
        }
        catch (Exception)
        {
            Console.WriteLine("Unexpected exception:");
            return 2;
        }


        Console.WriteLine("Test failed!");
        return 1;
    }
}
