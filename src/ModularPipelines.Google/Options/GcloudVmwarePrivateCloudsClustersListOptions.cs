using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "clusters", "list")]
public record GcloudVmwarePrivateCloudsClustersListOptions(
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--location")] string Location
) : GcloudOptions;