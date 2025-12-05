using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar-connections", "create-sync-configuration")]
public record AwsCodestarConnectionsCreateSyncConfigurationOptions(
[property: CliOption("--branch")] string Branch,
[property: CliOption("--config-file")] string ConfigFile,
[property: CliOption("--repository-link-id")] string RepositoryLinkId,
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--sync-type")] string SyncType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}