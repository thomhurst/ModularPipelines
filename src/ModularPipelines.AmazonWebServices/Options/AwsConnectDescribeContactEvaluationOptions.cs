using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "describe-contact-evaluation")]
public record AwsConnectDescribeContactEvaluationOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--evaluation-id")] string EvaluationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}