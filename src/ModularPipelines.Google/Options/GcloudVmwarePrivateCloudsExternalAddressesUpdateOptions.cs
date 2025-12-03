using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "external-addresses", "update")]
public record GcloudVmwarePrivateCloudsExternalAddressesUpdateOptions(
[property: CliArgument] string ExternalAddress,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateCloud
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--internal-ip")]
    public string? InternalIp { get; set; }
}