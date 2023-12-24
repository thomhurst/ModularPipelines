using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar-connections", "list-repository-sync-definitions")]
public record AwsCodestarConnectionsListRepositorySyncDefinitionsOptions(
[property: CommandSwitch("--repository-link-id")] string RepositoryLinkId,
[property: CommandSwitch("--sync-type")] string SyncType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}