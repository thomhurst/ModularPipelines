using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "adaptive-application-controls", "list")]
public record AzSecurityAdaptiveApplicationControlsListOptions : AzOptions;