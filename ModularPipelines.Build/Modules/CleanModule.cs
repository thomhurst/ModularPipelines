using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Modules;
using ModularPipelines.DotNet.Options;
using ParallelOptions = ModularPipelines.DotNet.Options.ParallelOptions;

namespace ModularPipelines.Build.Modules;

[DependsOn<RunUnitTestsModule>]
public class CleanModule : MultiDotNetModule
{
    public CleanModule(IModuleContext context) : base(context)
    {
    }

    protected override MultiDotNetModuleOptions Options
    {
        get => new()
            {
                Configuration = Configuration.Release,
                Command = new[] {"clean"},
                ParallelOptions = ParallelOptions.OneAtATime,
                WorkingDirectory = Context.Environment.GitRootDirectory!.FullName,
                ProjectsToInclude = path => path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
            };
        set { }
    }
}