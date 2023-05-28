using Pipeline.NET.Context;
using Pipeline.NET.DotNet.Modules;
using Pipeline.NET.DotNet.Options;
using ParallelOptions = Pipeline.NET.DotNet.Options.ParallelOptions;

namespace Pipeline.NET.Build.Modules;

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