using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ivs-realtime", "start-composition")]
public record AwsIvsRealtimeStartCompositionOptions(
[property: CommandSwitch("--destinations")] string[] Destinations,
[property: CommandSwitch("--stage-arn")] string StageArn
) : AwsOptions
{
    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--layout")]
    public string? Layout { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}