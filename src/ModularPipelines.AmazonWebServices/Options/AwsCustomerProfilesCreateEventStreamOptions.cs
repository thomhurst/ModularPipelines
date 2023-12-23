using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "create-event-stream")]
public record AwsCustomerProfilesCreateEventStreamOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--uri")] string Uri,
[property: CommandSwitch("--event-stream-name")] string EventStreamName
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}