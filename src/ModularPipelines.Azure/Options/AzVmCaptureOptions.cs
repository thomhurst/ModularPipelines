using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vm", "capture")]
public record AzVmCaptureOptions(
[property: CliOption("--vhd-name-prefix")] string VhdNamePrefix
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--overwrite")]
    public bool? Overwrite { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--storage-container")]
    public string? StorageContainer { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}