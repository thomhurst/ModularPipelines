using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "list-permission-sets-provisioned-to-account")]
public record AwsSsoAdminListPermissionSetsProvisionedToAccountOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--instance-arn")] string InstanceArn
) : AwsOptions
{
    [CommandSwitch("--provisioning-status")]
    public string? ProvisioningStatus { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}