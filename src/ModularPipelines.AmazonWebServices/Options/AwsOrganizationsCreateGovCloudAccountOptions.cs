using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("organizations", "create-gov-cloud-account")]
public record AwsOrganizationsCreateGovCloudAccountOptions(
[property: CommandSwitch("--email")] string Email,
[property: CommandSwitch("--account-name")] string AccountName
) : AwsOptions
{
    [CommandSwitch("--role-name")]
    public string? RoleName { get; set; }

    [CommandSwitch("--iam-user-access-to-billing")]
    public string? IamUserAccessToBilling { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}