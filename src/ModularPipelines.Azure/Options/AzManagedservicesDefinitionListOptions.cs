using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedservices", "definition", "list")]
public record AzManagedservicesDefinitionListOptions : AzOptions;