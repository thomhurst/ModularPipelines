using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "update-account-settings")]
public record AwsChimeUpdateAccountSettingsOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--account-settings")] string AccountSettings
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}