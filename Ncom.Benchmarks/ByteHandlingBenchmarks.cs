using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ncom.Benchmarks
{
    [MemoryDiagnoser]
    public class ByteHandlingBenchmarks
    {

        private const int BufferSize = 72;
        private const int Offset = 50;
        private static readonly int Value = 1234567890;

        private readonly byte[] buffer_array = new byte[BufferSize];


        [Benchmark(Baseline = true)]
        public void BitConverter_ArrayCopy()
        {
            Array.Copy(BitConverter.GetBytes(Value), 0, buffer_array, Offset, 4);
        }

        [Benchmark]
        public unsafe void Manual_Array()
        {
            buffer_array[Offset + 4] = (byte)((Value >> 24) & 0xFF);
            buffer_array[Offset + 5] = (byte)((Value >> 16) & 0xFF);
            buffer_array[Offset + 6] = (byte)((Value >> 8) & 0xFF);
            buffer_array[Offset + 7] = (byte)(Value & 0xFF);
        }

        [Benchmark]
        public unsafe void Manual_Span()
        {
            Span<byte> buffer_span = new Span<byte>(buffer_array);

            buffer_span[Offset + 4] = (byte)((Value >> 24) & 0xFF);
            buffer_span[Offset + 5] = (byte)((Value >> 16) & 0xFF);
            buffer_span[Offset + 6] = (byte)((Value >> 8) & 0xFF);
            buffer_span[Offset + 7] = (byte)(Value & 0xFF);
        }

        [Benchmark]
        public unsafe void Manual_Span_Pointer()
        {
            double valueCopy = Value;
            long value = *(long*)&valueCopy;
            Span<byte> buffer_span = new Span<byte>(buffer_array, Offset, buffer_array.Length - Offset);

            fixed (byte* ptr = buffer_span)
            {
                *((long*)ptr) = value;
            }
        }

        [Benchmark]
        public void WriteUnaligned_MemoryMarshal()
        {
            Span<byte> buffer_span = new Span<byte>(buffer_array, Offset, buffer_array.Length - Offset);

            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(buffer_span), Value);
        }

        [Benchmark]
        public void WriteUnaligned_MemoryMarshal_Reversed()
        {
            Span<byte> buffer_span = new Span<byte>(buffer_array, Offset, buffer_array.Length - Offset);

            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(buffer_span), Value);

            buffer_span.Slice(0, 4).Reverse();
        }
    }
}
