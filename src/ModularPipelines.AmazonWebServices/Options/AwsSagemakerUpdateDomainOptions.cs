using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-domain")]
public record AwsSagemakerUpdateDomainOptions(
[property: CliOption("--domain-id")] string DomainId
) : AwsOptions
{
    [CliOption("--default-user-settings")]
    public string? DefaultUserSettings { get; set; }

    [CliOption("--domain-settings-for-update")]
    public string? DomainSettingsForUpdate { get; set; }

    [CliOption("--app-security-group-management")]
    public string? AppSecurityGroupManagement { get; set; }

    [CliOption("--default-space-settings")]
    public string? DefaultSpaceSettings { get; set; }

    [CliOption("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CliOption("--app-network-access-type")]
    public string? AppNetworkAccessType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}