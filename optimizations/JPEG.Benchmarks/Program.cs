using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;

namespace JPEG.Benchmarks;

internal class Program
{
	public static void Main(string[] args)
	{
		var k = new ManualConfig().WithOption(ConfigOptions.DisableOptimizationsValidator, true)
			.AddLogger(ConsoleLogger.Default)
			.AddColumnProvider(DefaultColumnProviders.Instance);
		BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, k);
		
	}
}