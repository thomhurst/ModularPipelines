using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "env", "workload-profile", "add")]
public record AzContainerappEnvWorkloadProfileAddOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workload-profile-name")] string WorkloadProfileName
) : AzOptions
{
    [CommandSwitch("--max-nodes")]
    public string? MaxNodes { get; set; }

    [CommandSwitch("--min-nodes")]
    public string? MinNodes { get; set; }

    [CommandSwitch("--workload-profile-type")]
    public string? WorkloadProfileType { get; set; }
}