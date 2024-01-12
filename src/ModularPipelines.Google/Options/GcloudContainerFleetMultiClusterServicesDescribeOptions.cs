using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "multi-cluster-services", "describe")]
public record GcloudContainerFleetMultiClusterServicesDescribeOptions : GcloudOptions;