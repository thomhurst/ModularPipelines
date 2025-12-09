using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "delete-rule")]
public record AwsEventsDeleteRuleOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--event-bus-name")]
    public string? EventBusName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}