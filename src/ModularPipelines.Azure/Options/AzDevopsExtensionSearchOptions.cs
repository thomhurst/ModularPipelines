using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "extension", "search")]
public record AzDevopsExtensionSearchOptions(
[property: CliOption("--search-query")] string SearchQuery
) : AzOptions;