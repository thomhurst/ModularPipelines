using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("stack-hci-vm", "disk", "detach")]
public record AzStackHciVmDiskDetachOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-name")] string VmName
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}