using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "mesh", "describe")]
public record GcloudContainerHubMeshDescribeOptions : GcloudOptions;