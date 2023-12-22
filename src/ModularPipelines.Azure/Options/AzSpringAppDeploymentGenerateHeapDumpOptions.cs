using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "app", "deployment", "generate-heap-dump")]
public record AzSpringAppDeploymentGenerateHeapDumpOptions(
[property: CommandSwitch("--app")] string App,
[property: CommandSwitch("--app-instance")] string AppInstance,
[property: CommandSwitch("--file-path")] string FilePath,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [CommandSwitch("--deployment")]
    public string? Deployment { get; set; }
}