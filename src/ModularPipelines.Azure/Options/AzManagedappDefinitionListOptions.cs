using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("managedapp", "definition", "list")]
public record AzManagedappDefinitionListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;