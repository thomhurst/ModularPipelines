using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "delete")]
public record GcloudVmwarePrivateCloudsDeleteOptions(
[property: CliArgument] string PrivateCloud,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--delay-hours")]
    public string? DelayHours { get; set; }
}