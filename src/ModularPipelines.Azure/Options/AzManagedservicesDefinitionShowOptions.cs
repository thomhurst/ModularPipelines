using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("managedservices", "definition", "show")]
public record AzManagedservicesDefinitionShowOptions(
[property: CliOption("--definition")] string Definition
) : AzOptions;