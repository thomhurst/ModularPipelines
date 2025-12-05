using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "enable-ipam-organization-admin-account")]
public record AwsEc2EnableIpamOrganizationAdminAccountOptions(
[property: CliOption("--delegated-admin-account-id")] string DelegatedAdminAccountId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}