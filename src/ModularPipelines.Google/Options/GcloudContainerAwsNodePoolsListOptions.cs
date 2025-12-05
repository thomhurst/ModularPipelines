using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "aws", "node-pools", "list")]
public record GcloudContainerAwsNodePoolsListOptions(
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--location")] string Location
) : GcloudOptions;