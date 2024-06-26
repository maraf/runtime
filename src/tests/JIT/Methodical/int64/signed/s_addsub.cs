// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit;

namespace JitTest_s_addsub_signed_cs
{
    public class Test
    {
        private static void testNumbers(long a, long b)
        {
            long c = 0;
            try
            {
                c = checked(a + b);
            }
            catch (OverflowException)
            {
                bool negative = false;
                ulong au, bu;

                if (a < 0)
                {
                    negative = !negative;
                    au = checked((ulong)(-a));
                }
                else
                {
                    au = checked((ulong)a);
                }
                if (b < 0)
                {
                    negative = !negative;
                    bu = checked((ulong)(-b));
                }
                else
                {
                    bu = checked((ulong)b);
                }

                ulong AH = au >> 32;
                ulong AL = au & 0xffffffff;
                ulong BH = bu >> 32;
                ulong BL = bu & 0xffffffff;

                ulong L = checked(BL + AL);
                ulong H = checked(AH + BH);
                if (H < 0x100000000)
                {
                    if (L >= 0x100000000)
                    {
                        checked
                        {
                            H++;
                            L -= 0x100000000;
                            if (L >= 0x100000000)
                                throw new Exception();
                        }
                    }
                    if (checked(L <= 0xffffffffffffffff - (H << 32)))
                        throw new Exception();
                }
                return;
            }
            if (checked(c - b != a || c - a != b))
                throw new Exception();
        }

        [Fact]
        [OuterLoop]
        public static int TestEntryPoint()
        {
            try
            {
                testNumbers(unchecked((long)0x0000000000000009), unchecked((long)0x00000000000000b8));
                testNumbers(unchecked((long)0x0000000000000009), unchecked((long)0x00000000000000f9));
                testNumbers(unchecked((long)0x000000000000006e), unchecked((long)0x0000000000000093));
                testNumbers(unchecked((long)0x000000000000001e), unchecked((long)0x0000000000000086));
                testNumbers(unchecked((long)0x00000000000000cc), unchecked((long)0x000000000000583f));
                testNumbers(unchecked((long)0x00000000000000c9), unchecked((long)0x000000000000a94c));
                testNumbers(unchecked((long)0x0000000000000054), unchecked((long)0x0000000000002d06));
                testNumbers(unchecked((long)0x0000000000000030), unchecked((long)0x0000000000009921));
                testNumbers(unchecked((long)0x000000000000001d), unchecked((long)0x0000000000450842));
                testNumbers(unchecked((long)0x000000000000002a), unchecked((long)0x0000000000999f6c));
                testNumbers(unchecked((long)0x00000000000000c5), unchecked((long)0x000000000090faa7));
                testNumbers(unchecked((long)0x0000000000000050), unchecked((long)0x000000000069de08));
                testNumbers(unchecked((long)0x000000000000009a), unchecked((long)0x000000000cd715be));
                testNumbers(unchecked((long)0x0000000000000039), unchecked((long)0x0000000016a61eb5));
                testNumbers(unchecked((long)0x00000000000000e0), unchecked((long)0x0000000095575fef));
                testNumbers(unchecked((long)0x0000000000000093), unchecked((long)0x00000000209e58c5));
                testNumbers(unchecked((long)0x000000000000003b), unchecked((long)0x0000000c3c34b48c));
                testNumbers(unchecked((long)0x00000000000000c2), unchecked((long)0x0000006a671c470f));
                testNumbers(unchecked((long)0x000000000000004b), unchecked((long)0x000000f538cede2b));
                testNumbers(unchecked((long)0x0000000000000099), unchecked((long)0x0000005ba885d43b));
                testNumbers(unchecked((long)0x0000000000000068), unchecked((long)0x00009f692f98ac45));
                testNumbers(unchecked((long)0x00000000000000d9), unchecked((long)0x00008d5eaa7f0a8e));
                testNumbers(unchecked((long)0x00000000000000ac), unchecked((long)0x0000ba1316512e4c));
                testNumbers(unchecked((long)0x000000000000001c), unchecked((long)0x00008c4fbf2f14aa));
                testNumbers(unchecked((long)0x00000000000000c0), unchecked((long)0x0069a9eb9a9bc822));
                testNumbers(unchecked((long)0x0000000000000074), unchecked((long)0x003f8f5a893de200));
                testNumbers(unchecked((long)0x0000000000000027), unchecked((long)0x000650eb1747a5bc));
                testNumbers(unchecked((long)0x00000000000000d9), unchecked((long)0x00d3d50809c70fda));
                testNumbers(unchecked((long)0x00000000000000c0), unchecked((long)0xac6556a4ca94513e));
                testNumbers(unchecked((long)0x0000000000000020), unchecked((long)0xa697fcbfd6d232d1));
                testNumbers(unchecked((long)0x000000000000009c), unchecked((long)0xc4421a4f5147b9b8));
                testNumbers(unchecked((long)0x000000000000009e), unchecked((long)0xc5ef494112a7b33f));
                testNumbers(unchecked((long)0x000000000000f7fa), unchecked((long)0x00000000000000af));
                testNumbers(unchecked((long)0x000000000000ad17), unchecked((long)0x00000000000000e8));
                testNumbers(unchecked((long)0x000000000000c9c4), unchecked((long)0x0000000000000045));
                testNumbers(unchecked((long)0x000000000000a704), unchecked((long)0x0000000000000012));
                testNumbers(unchecked((long)0x000000000000c55b), unchecked((long)0x000000000000a33a));
                testNumbers(unchecked((long)0x000000000000ab88), unchecked((long)0x0000000000009a3c));
                testNumbers(unchecked((long)0x000000000000a539), unchecked((long)0x000000000000cf3a));
                testNumbers(unchecked((long)0x0000000000005890), unchecked((long)0x000000000000eec8));
                testNumbers(unchecked((long)0x000000000000e9e2), unchecked((long)0x0000000000fe7c46));
                testNumbers(unchecked((long)0x0000000000007303), unchecked((long)0x0000000000419f2a));
                testNumbers(unchecked((long)0x000000000000e105), unchecked((long)0x000000000013f913));
                testNumbers(unchecked((long)0x0000000000008191), unchecked((long)0x0000000000fa2458));
                testNumbers(unchecked((long)0x00000000000006d9), unchecked((long)0x0000000091cf14f7));
                testNumbers(unchecked((long)0x000000000000bdb1), unchecked((long)0x0000000086c2a97c));
                testNumbers(unchecked((long)0x000000000000e905), unchecked((long)0x0000000064f702f4));
                testNumbers(unchecked((long)0x0000000000002fdc), unchecked((long)0x00000000f059caf6));
                testNumbers(unchecked((long)0x000000000000f8fd), unchecked((long)0x00000013f0265b1e));
                testNumbers(unchecked((long)0x000000000000e8b8), unchecked((long)0x0000000aa69a6308));
                testNumbers(unchecked((long)0x0000000000003d00), unchecked((long)0x000000fbcb67879b));
                testNumbers(unchecked((long)0x000000000000aa46), unchecked((long)0x00000085c3d371d5));
                testNumbers(unchecked((long)0x0000000000005f60), unchecked((long)0x000008cde4a63203));
                testNumbers(unchecked((long)0x00000000000092b5), unchecked((long)0x00007ca86ba2f30e));
                testNumbers(unchecked((long)0x00000000000093c6), unchecked((long)0x0000a2d73fc4eac0));
                testNumbers(unchecked((long)0x0000000000004156), unchecked((long)0x000006dbd08f2fda));
                testNumbers(unchecked((long)0x0000000000004597), unchecked((long)0x006cfb0ba5962826));
                testNumbers(unchecked((long)0x0000000000006bac), unchecked((long)0x001e79315071480f));
                testNumbers(unchecked((long)0x0000000000002c3a), unchecked((long)0x0092f12cbd82df69));
                testNumbers(unchecked((long)0x0000000000009859), unchecked((long)0x00b0f0cd9dc019f2));
                testNumbers(unchecked((long)0x000000000000b37f), unchecked((long)0x4966447d15850076));
                testNumbers(unchecked((long)0x0000000000005e34), unchecked((long)0x7c1869c9ed2cad38));
                testNumbers(unchecked((long)0x0000000000005c54), unchecked((long)0x7cee70ee82837a08));
                testNumbers(unchecked((long)0x000000000000967f), unchecked((long)0x4eb98adf4b8b0d32));
                testNumbers(unchecked((long)0x0000000000fd2919), unchecked((long)0x000000000000005d));
                testNumbers(unchecked((long)0x0000000000abd5b1), unchecked((long)0x0000000000000098));
                testNumbers(unchecked((long)0x0000000000ab1887), unchecked((long)0x00000000000000ef));
                testNumbers(unchecked((long)0x000000000096034a), unchecked((long)0x000000000000002f));
                testNumbers(unchecked((long)0x0000000000d5bb94), unchecked((long)0x00000000000057d2));
                testNumbers(unchecked((long)0x0000000000d7b2cb), unchecked((long)0x00000000000080f5));
                testNumbers(unchecked((long)0x00000000004ccc6d), unchecked((long)0x000000000000087c));
                testNumbers(unchecked((long)0x0000000000ec0c50), unchecked((long)0x000000000000bdff));
                testNumbers(unchecked((long)0x00000000008a6865), unchecked((long)0x000000000076c014));
                testNumbers(unchecked((long)0x0000000000ac38dd), unchecked((long)0x0000000000f12b09));
                testNumbers(unchecked((long)0x0000000000615e2a), unchecked((long)0x0000000000e7cbf8));
                testNumbers(unchecked((long)0x00000000000e214f), unchecked((long)0x00000000005b8e2f));
                testNumbers(unchecked((long)0x00000000003bd7c6), unchecked((long)0x00000000c1db4e46));
                testNumbers(unchecked((long)0x0000000000ae208d), unchecked((long)0x0000000001c9aa7a));
                testNumbers(unchecked((long)0x00000000008a9cef), unchecked((long)0x0000000003930b07));
                testNumbers(unchecked((long)0x000000000036b866), unchecked((long)0x00000000d64b7bef));
                testNumbers(unchecked((long)0x0000000000d337cd), unchecked((long)0x000000a2b45fb7de));
                testNumbers(unchecked((long)0x0000000000024471), unchecked((long)0x0000005c5de3da89));
                testNumbers(unchecked((long)0x0000000000012b15), unchecked((long)0x0000007cd40030fe));
                testNumbers(unchecked((long)0x0000000000d38af2), unchecked((long)0x0000005905921572));
                testNumbers(unchecked((long)0x0000000000aca0d7), unchecked((long)0x0000c632301abeb8));
                testNumbers(unchecked((long)0x00000000004eadc2), unchecked((long)0x00006a1ebf37403c));
                testNumbers(unchecked((long)0x00000000005d909c), unchecked((long)0x00004021bfa15862));
                testNumbers(unchecked((long)0x0000000000710e08), unchecked((long)0x0000e9a1a030b230));
                testNumbers(unchecked((long)0x0000000000478b9b), unchecked((long)0x00804add8afc31d9));
                testNumbers(unchecked((long)0x00000000005754ed), unchecked((long)0x00af85e7ebb1ce33));
                testNumbers(unchecked((long)0x00000000003ab44e), unchecked((long)0x00f41b9f70360f78));
                testNumbers(unchecked((long)0x00000000007aa129), unchecked((long)0x00eb6e4eddf7eb87));
                testNumbers(unchecked((long)0x00000000003b036f), unchecked((long)0x333874e4330fbfa4));
                testNumbers(unchecked((long)0x0000000000a33186), unchecked((long)0xec8607412503fc4c));
                testNumbers(unchecked((long)0x00000000009af471), unchecked((long)0xe7ad0935fdbff151));
                testNumbers(unchecked((long)0x0000000000c04e8c), unchecked((long)0x58ee406ab936ac24));
                testNumbers(unchecked((long)0x0000000054fdd28b), unchecked((long)0x0000000000000034));
                testNumbers(unchecked((long)0x0000000033736b36), unchecked((long)0x00000000000000fd));
                testNumbers(unchecked((long)0x0000000069cfe4b7), unchecked((long)0x0000000000000026));
                testNumbers(unchecked((long)0x00000000fd078d36), unchecked((long)0x00000000000000dc));
                testNumbers(unchecked((long)0x0000000075cc3f36), unchecked((long)0x0000000000001617));
                testNumbers(unchecked((long)0x00000000075d660e), unchecked((long)0x0000000000008511));
                testNumbers(unchecked((long)0x0000000052acb037), unchecked((long)0x00000000000043cb));
                testNumbers(unchecked((long)0x00000000a0db7bf5), unchecked((long)0x0000000000002c98));
                testNumbers(unchecked((long)0x0000000083d4be11), unchecked((long)0x0000000000ba37c9));
                testNumbers(unchecked((long)0x0000000083d04f94), unchecked((long)0x00000000003ddbd0));
                testNumbers(unchecked((long)0x000000005ed41f6a), unchecked((long)0x0000000000eaf1d5));
                testNumbers(unchecked((long)0x000000000e364a9a), unchecked((long)0x000000000085880c));
                testNumbers(unchecked((long)0x0000000012657ecb), unchecked((long)0x00000000a88b8a68));
                testNumbers(unchecked((long)0x000000009897a4ac), unchecked((long)0x0000000076707981));
                testNumbers(unchecked((long)0x00000000469cd1cf), unchecked((long)0x00000000cf40f67a));
                testNumbers(unchecked((long)0x00000000ee7444c8), unchecked((long)0x00000000d1b0d7de));
                testNumbers(unchecked((long)0x00000000fbb6f547), unchecked((long)0x000000c1ef3c4d9b));
                testNumbers(unchecked((long)0x000000000e20dd53), unchecked((long)0x000000b05833c7cf));
                testNumbers(unchecked((long)0x00000000e5733fb8), unchecked((long)0x0000008eae18a855));
                testNumbers(unchecked((long)0x000000005db1c271), unchecked((long)0x000000c4a2f7c27d));
                testNumbers(unchecked((long)0x0000000007add22a), unchecked((long)0x00000ed9fd23dc3e));
                testNumbers(unchecked((long)0x000000002239d1d5), unchecked((long)0x0000a1ae07a62635));
                testNumbers(unchecked((long)0x00000000410d4d58), unchecked((long)0x0000c05c5205bed2));
                testNumbers(unchecked((long)0x000000004c3c435e), unchecked((long)0x00001e30c1bf628a));
                testNumbers(unchecked((long)0x00000000096f44d5), unchecked((long)0x005488c521a6072b));
                testNumbers(unchecked((long)0x0000000017f28913), unchecked((long)0x00796ff3891c44ff));
                testNumbers(unchecked((long)0x0000000065be69cf), unchecked((long)0x00dd5c6f9b3f3119));
                testNumbers(unchecked((long)0x000000002200f221), unchecked((long)0x00ab6c98c90cfe9d));
                testNumbers(unchecked((long)0x00000000d48bee1a), unchecked((long)0x64b76d7491a58799));
                testNumbers(unchecked((long)0x000000006cb93100), unchecked((long)0xa515fe27402dad45));
                testNumbers(unchecked((long)0x00000000bed95abe), unchecked((long)0xc9924098acc74be9));
                testNumbers(unchecked((long)0x0000000092781a2e), unchecked((long)0x67ada9ef3f9e39b7));
                testNumbers(unchecked((long)0x000000e3aafcdae2), unchecked((long)0x000000000000009c));
                testNumbers(unchecked((long)0x000000d8dad80c34), unchecked((long)0x0000000000000099));
                testNumbers(unchecked((long)0x000000addcd074d6), unchecked((long)0x00000000000000ea));
                testNumbers(unchecked((long)0x00000096735bc25a), unchecked((long)0x00000000000000ba));
                testNumbers(unchecked((long)0x000000f492ef7446), unchecked((long)0x00000000000039b1));
                testNumbers(unchecked((long)0x000000bc86816119), unchecked((long)0x0000000000001520));
                testNumbers(unchecked((long)0x00000060a36818e7), unchecked((long)0x000000000000c5a8));
                testNumbers(unchecked((long)0x000000317121d508), unchecked((long)0x000000000000ac3d));
                testNumbers(unchecked((long)0x0000004abfdaf232), unchecked((long)0x00000000005cea57));
                testNumbers(unchecked((long)0x000000acc458f392), unchecked((long)0x0000000000a9c3e3));
                testNumbers(unchecked((long)0x0000001020993532), unchecked((long)0x0000000000df6042));
                testNumbers(unchecked((long)0x000000ad25b80abb), unchecked((long)0x0000000000cec15b));
                testNumbers(unchecked((long)0x0000002305d2c443), unchecked((long)0x000000002a26131c));
                testNumbers(unchecked((long)0x00000007c42e2ce0), unchecked((long)0x000000009768024f));
                testNumbers(unchecked((long)0x00000076f674816c), unchecked((long)0x000000008d33c7b4));
                testNumbers(unchecked((long)0x000000bf567b23bc), unchecked((long)0x00000000ef264890));
                testNumbers(unchecked((long)0x000000e3283681a0), unchecked((long)0x0000002e66850719));
                testNumbers(unchecked((long)0x000000011fe13754), unchecked((long)0x00000066fad0b407));
                testNumbers(unchecked((long)0x00000052f259009f), unchecked((long)0x000000a2886ef414));
                testNumbers(unchecked((long)0x000000a9ebb540fc), unchecked((long)0x0000009d27ba694f));
                testNumbers(unchecked((long)0x00000083af60d7eb), unchecked((long)0x0000b6f2a0f51f4c));
                testNumbers(unchecked((long)0x000000f2ec42d13a), unchecked((long)0x000046855f279407));
                testNumbers(unchecked((long)0x00000094e71cb562), unchecked((long)0x00002d9566618e56));
                testNumbers(unchecked((long)0x000000c0ee690ddc), unchecked((long)0x000054295c8ca584));
                testNumbers(unchecked((long)0x0000002683cd5206), unchecked((long)0x00a5a2d269bcd188));
                testNumbers(unchecked((long)0x0000002e77038305), unchecked((long)0x00c727f0f3787e22));
                testNumbers(unchecked((long)0x0000008323b9d026), unchecked((long)0x00fed29f8575c120));
                testNumbers(unchecked((long)0x0000007b3231f0fc), unchecked((long)0x0091080854b27d3e));
                testNumbers(unchecked((long)0x00000084522a7708), unchecked((long)0x91ba8f22fccd6222));
                testNumbers(unchecked((long)0x000000afb1b50d90), unchecked((long)0x3261a532b65c7838));
                testNumbers(unchecked((long)0x0000002c65e838c6), unchecked((long)0x5b858452c9bf6f39));
                testNumbers(unchecked((long)0x000000219e837734), unchecked((long)0x97873bed5bb0a44b));
                testNumbers(unchecked((long)0x00009f133e2f116f), unchecked((long)0x0000000000000073));
                testNumbers(unchecked((long)0x0000887577574766), unchecked((long)0x0000000000000048));
                testNumbers(unchecked((long)0x0000ba4c778d4aa8), unchecked((long)0x000000000000003a));
                testNumbers(unchecked((long)0x00002683df421474), unchecked((long)0x0000000000000056));
                testNumbers(unchecked((long)0x00006ff76294c275), unchecked((long)0x00000000000089f7));
                testNumbers(unchecked((long)0x0000fdf053abefa2), unchecked((long)0x000000000000eb65));
                testNumbers(unchecked((long)0x0000ea4b254b24eb), unchecked((long)0x000000000000ba27));
                testNumbers(unchecked((long)0x000009f7ce21b811), unchecked((long)0x000000000000e8f6));
                testNumbers(unchecked((long)0x00009cc645fa08a1), unchecked((long)0x0000000000a29ea3));
                testNumbers(unchecked((long)0x0000726f9a9f816e), unchecked((long)0x000000000070dce1));
                testNumbers(unchecked((long)0x0000a4be34825ef6), unchecked((long)0x0000000000bb2be7));
                testNumbers(unchecked((long)0x000057ff147cb7c1), unchecked((long)0x0000000000e255af));
                testNumbers(unchecked((long)0x0000ab9d6f546dd4), unchecked((long)0x000000007e2772a5));
                testNumbers(unchecked((long)0x0000b148e3446e89), unchecked((long)0x0000000051ed3c28));
                testNumbers(unchecked((long)0x00001e3abfe9725e), unchecked((long)0x00000000d4dec3f4));
                testNumbers(unchecked((long)0x0000f61bcaba115e), unchecked((long)0x00000000fade149f));
                testNumbers(unchecked((long)0x0000ae642b9a6626), unchecked((long)0x000000d8de0e0b9a));
                testNumbers(unchecked((long)0x00009d015a13c8ae), unchecked((long)0x000000afc8827997));
                testNumbers(unchecked((long)0x0000ecc72cc2df89), unchecked((long)0x00000070d47ec7c4));
                testNumbers(unchecked((long)0x0000fdbf05894fd2), unchecked((long)0x00000012aec393bd));
                testNumbers(unchecked((long)0x0000cd7675a70874), unchecked((long)0x0000d7d696a62cbc));
                testNumbers(unchecked((long)0x0000fad44a89216d), unchecked((long)0x0000cb8cfc8ada4c));
                testNumbers(unchecked((long)0x0000f41eb5363551), unchecked((long)0x00009c040aa7775e));
                testNumbers(unchecked((long)0x00003c02d93e01f6), unchecked((long)0x0000f1f4e68a14f8));
                testNumbers(unchecked((long)0x0000e0d99954b598), unchecked((long)0x00b2a2de4e453485));
                testNumbers(unchecked((long)0x0000a6081be866d9), unchecked((long)0x00f2a12e845e4f2e));
                testNumbers(unchecked((long)0x0000ae56a5680dfd), unchecked((long)0x00c96cd7c15d5bec));
                testNumbers(unchecked((long)0x0000360363e37938), unchecked((long)0x00d4ed572e1937e0));
                testNumbers(unchecked((long)0x00001f052aebf185), unchecked((long)0x3584e582d1c6db1a));
                testNumbers(unchecked((long)0x00003fac9c7b3d1b), unchecked((long)0xa4b120f080d69113));
                testNumbers(unchecked((long)0x00005330d51c3217), unchecked((long)0xc16dd32ffd822c0e));
                testNumbers(unchecked((long)0x0000cd0694ff5ab0), unchecked((long)0x29673fe67245fbfc));
                testNumbers(unchecked((long)0x0098265e5a308523), unchecked((long)0x000000000000007d));
                testNumbers(unchecked((long)0x00560863350df217), unchecked((long)0x00000000000000c8));
                testNumbers(unchecked((long)0x00798ce804d829a1), unchecked((long)0x00000000000000b1));
                testNumbers(unchecked((long)0x007994c0051256fd), unchecked((long)0x000000000000005c));
                testNumbers(unchecked((long)0x00ff1a2838e69f42), unchecked((long)0x0000000000003c16));
                testNumbers(unchecked((long)0x009e7e95ac5de2c7), unchecked((long)0x000000000000ed49));
                testNumbers(unchecked((long)0x00fd6867eabba5c0), unchecked((long)0x000000000000c689));
                testNumbers(unchecked((long)0x009d1632daf20de0), unchecked((long)0x000000000000b74f));
                testNumbers(unchecked((long)0x00ee29d8f76d4e9c), unchecked((long)0x00000000008020d4));
                testNumbers(unchecked((long)0x0089e03ecf8daa0a), unchecked((long)0x00000000003e7587));
                testNumbers(unchecked((long)0x00115763be4beb44), unchecked((long)0x000000000088f762));
                testNumbers(unchecked((long)0x00815cfc87c427d0), unchecked((long)0x00000000009eec06));
                testNumbers(unchecked((long)0x001d9c3c9ded0c1a), unchecked((long)0x00000000b9f6d331));
                testNumbers(unchecked((long)0x00932225412f1222), unchecked((long)0x00000000130ff743));
                testNumbers(unchecked((long)0x00fe82151e2e0bf3), unchecked((long)0x00000000781cd6f9));
                testNumbers(unchecked((long)0x002222abb5061b12), unchecked((long)0x000000000491f1df));
                testNumbers(unchecked((long)0x0012ce0cf0452748), unchecked((long)0x000000a8566274aa));
                testNumbers(unchecked((long)0x00e570484e9937e1), unchecked((long)0x000000ac81f171be));
                testNumbers(unchecked((long)0x00eb371f7f8f514e), unchecked((long)0x000000df0248189c));
                testNumbers(unchecked((long)0x003777a7cc43dfd7), unchecked((long)0x0000003a7b8eaf40));
                testNumbers(unchecked((long)0x00e181db76238786), unchecked((long)0x00004126e572a568));
                testNumbers(unchecked((long)0x00ac1df87977e122), unchecked((long)0x0000e1e8cfde6678));
                testNumbers(unchecked((long)0x001c858763a2c23b), unchecked((long)0x000004ef61f3964f));
                testNumbers(unchecked((long)0x00bd786bbb71ce46), unchecked((long)0x00002cda097a464f));
                testNumbers(unchecked((long)0x00a7a6de21a46360), unchecked((long)0x00007afda16f98c3));
                testNumbers(unchecked((long)0x006fed70a6ccfdf2), unchecked((long)0x009771441e8e00e8));
                testNumbers(unchecked((long)0x005ad2782dcd5e60), unchecked((long)0x000d170d518385f6));
                testNumbers(unchecked((long)0x001fd67b153bc9b9), unchecked((long)0x007b3366dff66c6c));
                testNumbers(unchecked((long)0x00bf00203beb73f4), unchecked((long)0x693495fefab1c77e));
                testNumbers(unchecked((long)0x002faac1b1b068f8), unchecked((long)0x1cb11cc5c3aaff86));
                testNumbers(unchecked((long)0x00bb63cfbffe7648), unchecked((long)0x84f5b0c583f9e77b));
                testNumbers(unchecked((long)0x00615db89673241c), unchecked((long)0x8de5f125247eba0f));
                testNumbers(unchecked((long)0x9be183a6b293dffe), unchecked((long)0x0000000000000072));
                testNumbers(unchecked((long)0xa3df9b76d8a51b19), unchecked((long)0x00000000000000c4));
                testNumbers(unchecked((long)0xb4cc300f0ea7566d), unchecked((long)0x000000000000007e));
                testNumbers(unchecked((long)0xfdac12a8e23e16e7), unchecked((long)0x0000000000000015));
                testNumbers(unchecked((long)0xc0805405aadc0f47), unchecked((long)0x00000000000019d4));
                testNumbers(unchecked((long)0x843a391f8d9f8972), unchecked((long)0x000000000000317a));
                testNumbers(unchecked((long)0x5a0d124c427ed453), unchecked((long)0x00000000000034fe));
                testNumbers(unchecked((long)0x8631150f34008f1b), unchecked((long)0x0000000000002ecd));
                testNumbers(unchecked((long)0x3ff4c18715ad3a76), unchecked((long)0x000000000072d22a));
                testNumbers(unchecked((long)0x3ef93e5a649422bd), unchecked((long)0x0000000000db5c60));
                testNumbers(unchecked((long)0x6bdd1056ae58fe0e), unchecked((long)0x0000000000805c75));
                testNumbers(unchecked((long)0xeff1fa30f3ad9ded), unchecked((long)0x00000000000c83ca));
                testNumbers(unchecked((long)0xbbc143ac147e56a9), unchecked((long)0x00000000161179b7));
                testNumbers(unchecked((long)0x0829dde88caa2e45), unchecked((long)0x000000001443ab62));
                testNumbers(unchecked((long)0x97ac43ff797a4514), unchecked((long)0x0000000033eef42b));
                testNumbers(unchecked((long)0x703e9cdf96a148aa), unchecked((long)0x000000008e08f3d8));
                testNumbers(unchecked((long)0x75cbb739b54e2ad6), unchecked((long)0x0000007a8b12628c));
                testNumbers(unchecked((long)0x91e42fafe97d638f), unchecked((long)0x0000000fbe867c51));
                testNumbers(unchecked((long)0x9159d77deec116c1), unchecked((long)0x00000096c0c774fc));
                testNumbers(unchecked((long)0xb59dbb4c15761d88), unchecked((long)0x0000004a033a73e7));
                testNumbers(unchecked((long)0xab668e9783af9617), unchecked((long)0x00005aa18404076c));
                testNumbers(unchecked((long)0x54c68e5b5c4127df), unchecked((long)0x0000f2934fd8dd1f));
                testNumbers(unchecked((long)0xf490d3936184c9f9), unchecked((long)0x00004007477e2110));
                testNumbers(unchecked((long)0x349e577c9d5c44e2), unchecked((long)0x0000bdb2235af963));
                testNumbers(unchecked((long)0x58f3ac26cdafde28), unchecked((long)0x0017d4f4ade9ec35));
                testNumbers(unchecked((long)0xa4a263c316d21f4c), unchecked((long)0x00a7ec1e6fda834b));
                testNumbers(unchecked((long)0x6ab14771c448666f), unchecked((long)0x005b0f49593c3a27));
                testNumbers(unchecked((long)0x15f392c3602aa4f7), unchecked((long)0x0018af171045f88e));
                testNumbers(unchecked((long)0xf17de69c0063f62c), unchecked((long)0xee2a164c2c3a46f8));
                testNumbers(unchecked((long)0xf34b743eeff8e5c6), unchecked((long)0x4f4067f1a0e404ad));
                testNumbers(unchecked((long)0xee0296f678756647), unchecked((long)0xf1bbfdc6f0280d36));
                testNumbers(unchecked((long)0x65c33db0c952b829), unchecked((long)0xa7ab9c39dcffbcf3));
                Console.WriteLine("All tests passed.");
                return 100;
            }
            catch (DivideByZeroException)
            {
                return 1;
            }
        }
    }
}
