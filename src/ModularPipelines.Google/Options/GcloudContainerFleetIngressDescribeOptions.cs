using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "ingress", "describe")]
public record GcloudContainerFleetIngressDescribeOptions : GcloudOptions;