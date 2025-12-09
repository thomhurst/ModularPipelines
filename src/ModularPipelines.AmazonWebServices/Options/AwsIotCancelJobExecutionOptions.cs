using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "cancel-job-execution")]
public record AwsIotCancelJobExecutionOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--thing-name")] string ThingName
) : AwsOptions
{
    [CliOption("--expected-version")]
    public long? ExpectedVersion { get; set; }

    [CliOption("--status-details")]
    public IEnumerable<KeyValue>? StatusDetails { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}