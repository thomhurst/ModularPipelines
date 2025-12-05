using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataexchange", "create-event-action")]
public record AwsDataexchangeCreateEventActionOptions(
[property: CliOption("--action")] string Action,
[property: CliOption("--event")] string Event
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}