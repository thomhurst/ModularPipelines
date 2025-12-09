using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("managedservices", "definition", "list")]
public record AzManagedservicesDefinitionListOptions : AzOptions;