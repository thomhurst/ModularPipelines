using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "list-permission-sets-provisioned-to-account")]
public record AwsSsoAdminListPermissionSetsProvisionedToAccountOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--instance-arn")] string InstanceArn
) : AwsOptions
{
    [CliOption("--provisioning-status")]
    public string? ProvisioningStatus { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}