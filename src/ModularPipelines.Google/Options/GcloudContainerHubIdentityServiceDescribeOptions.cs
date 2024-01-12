using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "identity-service", "describe")]
public record GcloudContainerHubIdentityServiceDescribeOptions : GcloudOptions;