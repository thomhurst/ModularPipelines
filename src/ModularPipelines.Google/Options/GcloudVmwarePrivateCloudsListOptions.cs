using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "list")]
public record GcloudVmwarePrivateCloudsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}