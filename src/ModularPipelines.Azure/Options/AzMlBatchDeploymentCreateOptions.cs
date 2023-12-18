using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "batch-deployment", "create")]
public record AzMlBatchDeploymentCreateOptions(
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [BooleanCommandSwitch("--set-default")]
    public bool? SetDefault { get; set; }

    [BooleanCommandSwitch("--skip-script-validation")]
    public bool? SkipScriptValidation { get; set; }
}