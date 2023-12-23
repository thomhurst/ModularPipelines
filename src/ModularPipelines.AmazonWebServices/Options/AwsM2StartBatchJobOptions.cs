using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("m2", "start-batch-job")]
public record AwsM2StartBatchJobOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--batch-job-identifier")] string BatchJobIdentifier
) : AwsOptions
{
    [CommandSwitch("--job-params")]
    public IEnumerable<KeyValue>? JobParams { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}