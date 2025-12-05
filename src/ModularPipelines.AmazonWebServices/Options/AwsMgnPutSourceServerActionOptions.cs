using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgn", "put-source-server-action")]
public record AwsMgnPutSourceServerActionOptions(
[property: CliOption("--action-id")] string ActionId,
[property: CliOption("--action-name")] string ActionName,
[property: CliOption("--document-identifier")] string DocumentIdentifier,
[property: CliOption("--order")] int Order,
[property: CliOption("--source-server-id")] string SourceServerId
) : AwsOptions
{
    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliOption("--category")]
    public string? Category { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--document-version")]
    public string? DocumentVersion { get; set; }

    [CliOption("--external-parameters")]
    public IEnumerable<KeyValue>? ExternalParameters { get; set; }

    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--timeout-seconds")]
    public int? TimeoutSeconds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}