using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("advisor", "configuration", "list")]
public record AzAdvisorConfigurationListOptions : AzOptions;