using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "delete-partner-event-source")]
public record AwsEventsDeletePartnerEventSourceOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--account")] string Account
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}