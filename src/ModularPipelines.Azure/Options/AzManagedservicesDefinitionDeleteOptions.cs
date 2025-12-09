using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("managedservices", "definition", "delete")]
public record AzManagedservicesDefinitionDeleteOptions(
[property: CliOption("--definition")] string Definition
) : AzOptions;