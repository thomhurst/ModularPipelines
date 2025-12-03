using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "list-event-buses")]
public record AwsEventsListEventBusesOptions : AwsOptions
{
    [CliOption("--name-prefix")]
    public string? NamePrefix { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}