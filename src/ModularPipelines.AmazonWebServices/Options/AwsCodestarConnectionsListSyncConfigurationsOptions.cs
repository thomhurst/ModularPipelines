using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar-connections", "list-sync-configurations")]
public record AwsCodestarConnectionsListSyncConfigurationsOptions(
[property: CommandSwitch("--repository-link-id")] string RepositoryLinkId,
[property: CommandSwitch("--sync-type")] string SyncType
) : AwsOptions
{
    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}