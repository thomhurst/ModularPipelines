using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "get-event-stream")]
public record AwsCustomerProfilesGetEventStreamOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--event-stream-name")] string EventStreamName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}