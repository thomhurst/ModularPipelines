using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("graph", "shared-query", "create")]
public record AzGraphSharedQueryCreateOptions(
[property: CliOption("--description")] string Description,
[property: CliOption("--graph-query")] string GraphQuery,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--tags")]
    public string? Tags { get; set; }
}