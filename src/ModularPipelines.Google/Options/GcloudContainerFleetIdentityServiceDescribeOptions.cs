using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "identity-service", "describe")]
public record GcloudContainerFleetIdentityServiceDescribeOptions : GcloudOptions;