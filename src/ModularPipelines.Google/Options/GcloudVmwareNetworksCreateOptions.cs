using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "networks", "create")]
public record GcloudVmwareNetworksCreateOptions(
[property: CliArgument] string VmwareEngineNetwork,
[property: CliArgument] string Location,
[property: CliOption("--type")] string Type
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }
}