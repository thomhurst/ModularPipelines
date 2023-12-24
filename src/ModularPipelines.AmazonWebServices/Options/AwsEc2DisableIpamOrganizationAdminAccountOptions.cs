using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "disable-ipam-organization-admin-account")]
public record AwsEc2DisableIpamOrganizationAdminAccountOptions(
[property: CommandSwitch("--delegated-admin-account-id")] string DelegatedAdminAccountId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}