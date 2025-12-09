using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar-connections", "update-sync-configuration")]
public record AwsCodestarConnectionsUpdateSyncConfigurationOptions(
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--sync-type")] string SyncType
) : AwsOptions
{
    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliOption("--config-file")]
    public string? ConfigFile { get; set; }

    [CliOption("--repository-link-id")]
    public string? RepositoryLinkId { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}