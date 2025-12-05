using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "external-addresses", "create")]
public record GcloudVmwarePrivateCloudsExternalAddressesCreateOptions(
[property: CliArgument] string ExternalAddress,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateCloud,
[property: CliOption("--internal-ip")] string InternalIp
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }
}