using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Benchmarks.Benchmarks;

namespace Benchmarks;

internal class Program
{
	static void Main(string[] args)
	{
		BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());
		//BenchmarkRunner.Run<MemoryTraffic>();
		//BenchmarkRunner.Run<StructVsClassBenchmark>();
		BenchmarkRunner.Run<ByteArrayEqualityBenchmark>();
		//BenchmarkRunner.Run<NewConstraintBenchmark>();
		//BenchmarkRunner.Run<MaxBenchmark>();


		
	}
}