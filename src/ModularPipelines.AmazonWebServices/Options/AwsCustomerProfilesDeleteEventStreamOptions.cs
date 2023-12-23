using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "delete-event-stream")]
public record AwsCustomerProfilesDeleteEventStreamOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--event-stream-name")] string EventStreamName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}