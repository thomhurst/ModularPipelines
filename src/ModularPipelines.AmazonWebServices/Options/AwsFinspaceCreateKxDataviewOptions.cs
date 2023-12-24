using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("finspace", "create-kx-dataview")]
public record AwsFinspaceCreateKxDataviewOptions(
[property: CommandSwitch("--environment-id")] string EnvironmentId,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--dataview-name")] string DataviewName,
[property: CommandSwitch("--az-mode")] string AzMode
) : AwsOptions
{
    [CommandSwitch("--availability-zone-id")]
    public string? AvailabilityZoneId { get; set; }

    [CommandSwitch("--changeset-id")]
    public string? ChangesetId { get; set; }

    [CommandSwitch("--segment-configurations")]
    public string[]? SegmentConfigurations { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}