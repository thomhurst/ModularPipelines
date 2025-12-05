using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("search", "query-key", "list")]
public record AzSearchQueryKeyListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions;