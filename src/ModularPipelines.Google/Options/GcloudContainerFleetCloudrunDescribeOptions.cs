using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "cloudrun", "describe")]
public record GcloudContainerFleetCloudrunDescribeOptions : GcloudOptions;