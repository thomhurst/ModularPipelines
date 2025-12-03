using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vm", "nic", "add")]
public record AzVmNicAddOptions(
[property: CliOption("--nics")] string Nics,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-name")] string VmName
) : AzOptions
{
    [CliOption("--primary-nic")]
    public string? PrimaryNic { get; set; }
}