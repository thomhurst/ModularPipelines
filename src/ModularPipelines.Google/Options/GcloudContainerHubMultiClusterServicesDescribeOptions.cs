using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "multi-cluster-services", "describe")]
public record GcloudContainerHubMultiClusterServicesDescribeOptions : GcloudOptions;