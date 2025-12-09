using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("advisor", "configuration", "list")]
public record AzAdvisorConfigurationListOptions : AzOptions;