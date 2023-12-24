using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "update-service-sync-config")]
public record AwsProtonUpdateServiceSyncConfigOptions(
[property: CommandSwitch("--branch")] string Branch,
[property: CommandSwitch("--file-path")] string FilePath,
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--repository-provider")] string RepositoryProvider,
[property: CommandSwitch("--service-name")] string ServiceName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}