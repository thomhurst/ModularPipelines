using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataexchange", "update-event-action")]
public record AwsDataexchangeUpdateEventActionOptions(
[property: CliOption("--event-action-id")] string EventActionId
) : AwsOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}