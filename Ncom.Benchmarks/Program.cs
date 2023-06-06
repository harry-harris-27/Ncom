using BenchmarkDotNet.Running;
using Ncom.Benchmarks;
using System;

BenchmarkRunner.Run<ByteHandlingBenchmarks>();
Console.ReadLine();