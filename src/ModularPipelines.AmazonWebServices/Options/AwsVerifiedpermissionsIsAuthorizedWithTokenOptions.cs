using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("verifiedpermissions", "is-authorized-with-token")]
public record AwsVerifiedpermissionsIsAuthorizedWithTokenOptions(
[property: CliOption("--policy-store-id")] string PolicyStoreId
) : AwsOptions
{
    [CliOption("--identity-token")]
    public string? IdentityToken { get; set; }

    [CliOption("--access-token")]
    public string? AccessToken { get; set; }

    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--resource")]
    public string? Resource { get; set; }

    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--entities")]
    public string? Entities { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}