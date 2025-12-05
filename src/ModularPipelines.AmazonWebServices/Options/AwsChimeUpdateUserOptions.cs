using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "update-user")]
public record AwsChimeUpdateUserOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--user-type")]
    public string? UserType { get; set; }

    [CliOption("--alexa-for-business-metadata")]
    public string? AlexaForBusinessMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}