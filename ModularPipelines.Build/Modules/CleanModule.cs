using CliWrap.Buffered;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[DependsOn<RunUnitTestsModule>]
public class CleanModule : Module<List<BufferedCommandResult>>
{
    public CleanModule(IModuleContext context) : base(context)
    {
    }

    protected override async Task<ModuleResult<List<BufferedCommandResult>>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var results = new List<BufferedCommandResult>();

        foreach (var projectFile in Context.Environment
                     .GitRootDirectory!
                     .GetFiles(file => file.Path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)))
        {
            results.Add(await Context.DotNet().Clean(new DotNetOptions
            {
                TargetPath = projectFile.Path
            }, cancellationToken));
        }

        return results;
    }
}