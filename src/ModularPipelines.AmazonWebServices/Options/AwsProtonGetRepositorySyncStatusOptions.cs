using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "get-repository-sync-status")]
public record AwsProtonGetRepositorySyncStatusOptions(
[property: CommandSwitch("--branch")] string Branch,
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--repository-provider")] string RepositoryProvider,
[property: CommandSwitch("--sync-type")] string SyncType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}