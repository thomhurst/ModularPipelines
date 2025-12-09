using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "update-user-settings")]
public record AwsChimeUpdateUserSettingsOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--user-settings")] string UserSettings
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}