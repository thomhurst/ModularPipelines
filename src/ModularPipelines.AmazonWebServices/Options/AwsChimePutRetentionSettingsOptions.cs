using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "put-retention-settings")]
public record AwsChimePutRetentionSettingsOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--retention-settings")] string RetentionSettings
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}