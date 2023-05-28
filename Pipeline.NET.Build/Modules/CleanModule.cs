using Pipeline.NET.Attributes;
using Pipeline.NET.Context;
using Pipeline.NET.DotNet.Modules;
using Pipeline.NET.DotNet.Options;
using ParallelOptions = Pipeline.NET.DotNet.Options.ParallelOptions;

namespace Pipeline.NET.Build.Modules;

[DependsOn<RunUnitTestsModule>]
public class CleanModule : MultiDotNetModule
{
    public CleanModule(IModuleContext context) : base(context)
    {
    }

    protected override MultiDotNetModuleOptions Options => new()
    {
        Configuration = Configuration.Release,
        Command = new[] {"clean"},
        ParallelOptions = ParallelOptions.Concurrently,
        WorkingDirectory = Context.Environment.GitRootDirectory!.FullName,
        ProjectsToInclude = path => path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
    };
    
    
}