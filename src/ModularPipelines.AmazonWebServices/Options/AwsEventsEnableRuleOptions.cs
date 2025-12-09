using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "enable-rule")]
public record AwsEventsEnableRuleOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--event-bus-name")]
    public string? EventBusName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}