using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "delete-event-stream")]
public record AwsCustomerProfilesDeleteEventStreamOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--event-stream-name")] string EventStreamName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}