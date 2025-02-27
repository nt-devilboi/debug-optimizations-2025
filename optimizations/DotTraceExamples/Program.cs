using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using DotTraceExamples.Programs;

namespace DotTraceExamples;

internal class Program
{
	public static void Main(string[] args)
	{
		BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());
		//ProgramRunner.Run(new ComplexOperationTestProgram());
		ProgramRunner.Run(new EdgePreservingSmoothingProgram());
		//ProgramRunner.Run(new MeanShiftProgram());
	}
}