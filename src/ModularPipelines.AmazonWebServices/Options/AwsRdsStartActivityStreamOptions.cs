using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "start-activity-stream")]
public record AwsRdsStartActivityStreamOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--mode")] string Mode,
[property: CliOption("--kms-key-id")] string KmsKeyId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}