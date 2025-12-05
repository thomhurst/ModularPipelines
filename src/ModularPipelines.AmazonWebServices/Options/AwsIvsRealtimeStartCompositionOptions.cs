using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivs-realtime", "start-composition")]
public record AwsIvsRealtimeStartCompositionOptions(
[property: CliOption("--destinations")] string[] Destinations,
[property: CliOption("--stage-arn")] string StageArn
) : AwsOptions
{
    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--layout")]
    public string? Layout { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}