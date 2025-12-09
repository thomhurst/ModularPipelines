using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "update-service-setting")]
public record AwsSsmUpdateServiceSettingOptions(
[property: CliOption("--setting-id")] string SettingId,
[property: CliOption("--setting-value")] string SettingValue
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}