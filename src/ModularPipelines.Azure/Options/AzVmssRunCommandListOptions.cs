using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmss", "run-command", "list")]
public record AzVmssRunCommandListOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vmss-name")] string VmssName
) : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }
}