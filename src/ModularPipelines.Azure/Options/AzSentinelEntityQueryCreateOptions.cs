using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "entity-query", "create")]
public record AzSentinelEntityQueryCreateOptions(
[property: CliOption("--entity-query-id")] string EntityQueryId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--activity")]
    public string? Activity { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }
}