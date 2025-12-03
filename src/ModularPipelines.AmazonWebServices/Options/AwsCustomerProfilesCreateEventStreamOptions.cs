using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "create-event-stream")]
public record AwsCustomerProfilesCreateEventStreamOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--uri")] string Uri,
[property: CliOption("--event-stream-name")] string EventStreamName
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}