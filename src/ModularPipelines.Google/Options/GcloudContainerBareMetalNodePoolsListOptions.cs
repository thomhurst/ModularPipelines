using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "node-pools", "list")]
public record GcloudContainerBareMetalNodePoolsListOptions(
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--location")] string Location
) : GcloudOptions;