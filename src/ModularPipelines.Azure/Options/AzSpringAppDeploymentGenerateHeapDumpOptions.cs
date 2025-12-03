using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "app", "deployment", "generate-heap-dump")]
public record AzSpringAppDeploymentGenerateHeapDumpOptions(
[property: CliOption("--app")] string App,
[property: CliOption("--app-instance")] string AppInstance,
[property: CliOption("--file-path")] string FilePath,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--deployment")]
    public string? Deployment { get; set; }
}