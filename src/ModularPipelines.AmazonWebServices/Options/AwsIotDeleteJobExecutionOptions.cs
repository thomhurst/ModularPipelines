using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "delete-job-execution")]
public record AwsIotDeleteJobExecutionOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--thing-name")] string ThingName,
[property: CliOption("--execution-number")] long ExecutionNumber
) : AwsOptions
{
    [CliOption("--namespace-id")]
    public string? NamespaceId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}