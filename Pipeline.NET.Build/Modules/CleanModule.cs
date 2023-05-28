using Pipeline.NET.Attributes;
using Pipeline.NET.Context;
using Pipeline.NET.DotNet.Modules;
using Pipeline.NET.DotNet.Options;

namespace Pipeline.NET.Build.Modules;

[DependsOn<RunUnitTestsModule>]
public class CleanModule : MultiDotNetModule
{
    public CleanModule(IModuleContext context) : base(context)
    {
    }

    protected override MultiDotNetModuleOptions Options => new MultiDotNetModuleOptions()
    {
        Configuration = Configuration.Release,
        Command = "clean",
        WorkingDirectory = Context.Environment.GitRootDirectory!.FullName,
        ProjectsToInclude = path => path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
    };
    
    
}