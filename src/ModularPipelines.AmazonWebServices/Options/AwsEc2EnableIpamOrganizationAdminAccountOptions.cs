using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "enable-ipam-organization-admin-account")]
public record AwsEc2EnableIpamOrganizationAdminAccountOptions(
[property: CommandSwitch("--delegated-admin-account-id")] string DelegatedAdminAccountId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}