using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "list-replays")]
public record AwsEventsListReplaysOptions : AwsOptions
{
    [CliOption("--name-prefix")]
    public string? NamePrefix { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--event-source-arn")]
    public string? EventSourceArn { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}