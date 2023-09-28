using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;
using TomLonghurst.EnumerableAsyncProcessor.Extensions;

namespace ModularPipelines.Build.Modules;

[DependsOn<CodeFormattedNicelyModule>]
public class RunUnitTestsModule : Module<DotNetTestResult[]>
{
    /// <inheritdoc/>
    protected override async Task<DotNetTestResult[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        return await context.Git().RootDirectory
            .GetFiles(file => file.Path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
                              && file.Path.Contains("UnitTests", StringComparison.OrdinalIgnoreCase))
            .ToAsyncProcessorBuilder()
            .SelectAsync(async unitTestProjectFile => await context.DotNet().Test(new DotNetTestOptions
            {
                TargetPath = unitTestProjectFile.Path,
                Framework = "net7.0",
                Collect = "XPlat Code Coverage",
                EnvironmentVariables = new Dictionary<string, string?>
                {
                    ["GITHUB_ACTIONS"] = null,
                },
            }, cancellationToken))
            .ProcessInParallel();
    }
}
