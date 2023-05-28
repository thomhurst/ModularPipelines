using ModularPipelines.Context;
using ModularPipelines.DotNet.Modules;
using ModularPipelines.DotNet.Options;
using ParallelOptions = ModularPipelines.DotNet.Options.ParallelOptions;

namespace ModularPipelines.Build.Modules;

public class RunUnitTestsModule : MultiDotNetModule
{
    public RunUnitTestsModule(IModuleContext context) : base(context)
    {
    }

    protected override MultiDotNetModuleOptions Options => new()
    {
        Command = new[] {"test"},
        ParallelOptions = ParallelOptions.Concurrently,
        WorkingDirectory = Context.Environment.GitRootDirectory!.FullName,
        ProjectsToInclude = path => path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase) 
                                    && path.Contains("UnitTests", StringComparison.OrdinalIgnoreCase)
    };
}