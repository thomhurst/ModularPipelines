using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "multi-cluster-services", "enable")]
public record GcloudContainerFleetMultiClusterServicesEnableOptions : GcloudOptions;