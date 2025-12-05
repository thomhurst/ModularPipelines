using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "azure", "node-pools", "list")]
public record GcloudContainerAzureNodePoolsListOptions(
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--location")] string Location
) : GcloudOptions;