using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[DependsOn<CodeFormattedNicelyModule>]
public class RunUnitTestsModule : Module<List<DotNetTestResult>>
{
    protected override async Task<ModuleResult<List<DotNetTestResult>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var results = new List<DotNetTestResult>();

        foreach (var unitTestProjectFile in context.Environment
                     .GitRootDirectory!
                     .GetFiles(file => file.Path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
                                       && file.Path.Contains("UnitTests", StringComparison.OrdinalIgnoreCase)))
        {
            results.Add(await context.DotNet().Test(new DotNetTestOptions
            {
                TargetPath = unitTestProjectFile.Path,
                Collect = "XPlat Code Coverage"
            }, cancellationToken));
        }

        return results;
    }
}
