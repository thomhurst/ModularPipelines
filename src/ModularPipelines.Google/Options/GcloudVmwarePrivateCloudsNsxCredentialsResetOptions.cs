using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "nsx", "credentials", "reset")]
public record GcloudVmwarePrivateCloudsNsxCredentialsResetOptions(
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--location")] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}