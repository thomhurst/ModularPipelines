using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "multi-cluster-services", "enable")]
public record GcloudContainerHubMultiClusterServicesEnableOptions : GcloudOptions;