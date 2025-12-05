using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "reset-password")]
public record AwsWorkmailResetPasswordOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--password")] string Password
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}