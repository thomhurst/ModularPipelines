using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "ingress", "describe")]
public record GcloudContainerHubIngressDescribeOptions : GcloudOptions;