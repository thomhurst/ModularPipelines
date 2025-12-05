using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "put-retention-policy")]
public record AwsLogsPutRetentionPolicyOptions(
[property: CliOption("--log-group-name")] string LogGroupName,
[property: CliOption("--retention-in-days")] int RetentionInDays
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}