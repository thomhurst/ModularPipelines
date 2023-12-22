using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("advisor", "configuration", "list")]
public record AzAdvisorConfigurationListOptions : AzOptions;