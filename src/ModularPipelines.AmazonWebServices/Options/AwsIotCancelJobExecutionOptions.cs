using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "cancel-job-execution")]
public record AwsIotCancelJobExecutionOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--thing-name")] string ThingName
) : AwsOptions
{
    [CommandSwitch("--expected-version")]
    public long? ExpectedVersion { get; set; }

    [CommandSwitch("--status-details")]
    public IEnumerable<KeyValue>? StatusDetails { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}