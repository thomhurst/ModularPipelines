using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "mesh", "describe")]
public record GcloudContainerFleetMeshDescribeOptions : GcloudOptions;