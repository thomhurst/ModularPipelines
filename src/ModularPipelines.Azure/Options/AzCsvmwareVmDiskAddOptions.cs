using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("csvmware", "vm", "disk", "add")]
public record AzCsvmwareVmDiskAddOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-name")] string VmName
) : AzOptions
{
    [CliOption("--controller")]
    public string? Controller { get; set; }

    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--size")]
    public string? Size { get; set; }
}