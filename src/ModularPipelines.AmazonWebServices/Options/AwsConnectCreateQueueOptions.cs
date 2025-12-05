using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "create-queue")]
public record AwsConnectCreateQueueOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--name")] string Name,
[property: CliOption("--hours-of-operation-id")] string HoursOfOperationId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--outbound-caller-config")]
    public string? OutboundCallerConfig { get; set; }

    [CliOption("--max-contacts")]
    public int? MaxContacts { get; set; }

    [CliOption("--quick-connect-ids")]
    public string[]? QuickConnectIds { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}