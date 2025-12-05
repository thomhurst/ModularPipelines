using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("m2", "start-batch-job")]
public record AwsM2StartBatchJobOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--batch-job-identifier")] string BatchJobIdentifier
) : AwsOptions
{
    [CliOption("--job-params")]
    public IEnumerable<KeyValue>? JobParams { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}