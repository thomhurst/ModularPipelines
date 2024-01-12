using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "cloudrun", "describe")]
public record GcloudContainerHubCloudrunDescribeOptions : GcloudOptions;