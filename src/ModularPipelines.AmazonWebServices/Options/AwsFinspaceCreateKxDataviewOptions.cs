using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "create-kx-dataview")]
public record AwsFinspaceCreateKxDataviewOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--dataview-name")] string DataviewName,
[property: CliOption("--az-mode")] string AzMode
) : AwsOptions
{
    [CliOption("--availability-zone-id")]
    public string? AvailabilityZoneId { get; set; }

    [CliOption("--changeset-id")]
    public string? ChangesetId { get; set; }

    [CliOption("--segment-configurations")]
    public string[]? SegmentConfigurations { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}