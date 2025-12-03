using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("graph", "shared-query", "list")]
public record AzGraphSharedQueryListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;