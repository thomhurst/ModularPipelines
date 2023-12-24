using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar-connections", "create-sync-configuration")]
public record AwsCodestarConnectionsCreateSyncConfigurationOptions(
[property: CommandSwitch("--branch")] string Branch,
[property: CommandSwitch("--config-file")] string ConfigFile,
[property: CommandSwitch("--repository-link-id")] string RepositoryLinkId,
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--sync-type")] string SyncType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}