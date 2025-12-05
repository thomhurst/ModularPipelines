using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "env", "workload-profile", "update")]
public record AzContainerappEnvWorkloadProfileUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workload-profile-name")] string WorkloadProfileName
) : AzOptions
{
    [CliOption("--max-nodes")]
    public string? MaxNodes { get; set; }

    [CliOption("--min-nodes")]
    public string? MinNodes { get; set; }
}