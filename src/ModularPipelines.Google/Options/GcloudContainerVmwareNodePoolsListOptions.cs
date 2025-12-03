using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "vmware", "node-pools", "list")]
public record GcloudContainerVmwareNodePoolsListOptions(
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--location")] string Location
) : GcloudOptions;