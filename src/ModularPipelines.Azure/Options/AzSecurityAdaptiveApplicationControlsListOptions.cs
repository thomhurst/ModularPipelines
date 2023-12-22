using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "adaptive-application-controls", "list")]
public record AzSecurityAdaptiveApplicationControlsListOptions : AzOptions;