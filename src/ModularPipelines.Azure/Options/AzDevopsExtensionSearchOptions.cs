using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "extension", "search")]
public record AzDevopsExtensionSearchOptions(
[property: CommandSwitch("--search-query")] string SearchQuery
) : AzOptions;