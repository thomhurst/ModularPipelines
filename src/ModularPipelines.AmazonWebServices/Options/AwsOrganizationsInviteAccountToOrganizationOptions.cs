using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("organizations", "invite-account-to-organization")]
public record AwsOrganizationsInviteAccountToOrganizationOptions(
[property: CliOption("--target")] string Target
) : AwsOptions
{
    [CliOption("--notes")]
    public string? Notes { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}