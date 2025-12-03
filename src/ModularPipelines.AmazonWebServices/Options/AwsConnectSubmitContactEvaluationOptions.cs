using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "submit-contact-evaluation")]
public record AwsConnectSubmitContactEvaluationOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--evaluation-id")] string EvaluationId
) : AwsOptions
{
    [CliOption("--answers")]
    public IEnumerable<KeyValue>? Answers { get; set; }

    [CliOption("--notes")]
    public IEnumerable<KeyValue>? Notes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}