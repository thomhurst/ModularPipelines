using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("organizations", "create-account")]
public record AwsOrganizationsCreateAccountOptions(
[property: CliOption("--email")] string Email,
[property: CliOption("--account-name")] string AccountName
) : AwsOptions
{
    [CliOption("--role-name")]
    public string? RoleName { get; set; }

    [CliOption("--iam-user-access-to-billing")]
    public string? IamUserAccessToBilling { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}