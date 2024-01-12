using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "identity-service", "describe")]
public record GcloudContainerFleetIdentityServiceDescribeOptions : GcloudOptions;