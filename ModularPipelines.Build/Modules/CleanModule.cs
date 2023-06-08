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
    protected override async Task<ModuleResult<List<BufferedCommandResult>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var results = new List<BufferedCommandResult>();

        var projectFiles = context.Environment
            .GitRootDirectory!
            .GetFiles(file => file.Path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase));

        await Parallel.ForEachAsync(projectFiles, cancellationToken, async (projectFile, token) =>
        {
            results.Add(await context.DotNet().Clean(new DotNetOptions
            {
                TargetPath = projectFile.Path
            }, cancellationToken));
        });

        return results;
    }
}