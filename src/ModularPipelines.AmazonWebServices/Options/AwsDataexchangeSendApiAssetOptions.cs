using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataexchange", "send-api-asset")]
public record AwsDataexchangeSendApiAssetOptions(
[property: CliOption("--asset-id")] string AssetId,
[property: CliOption("--data-set-id")] string DataSetId,
[property: CliOption("--revision-id")] string RevisionId
) : AwsOptions
{
    [CliOption("--body")]
    public string? Body { get; set; }

    [CliOption("--query-string-parameters")]
    public IEnumerable<KeyValue>? QueryStringParameters { get; set; }

    [CliOption("--request-headers")]
    public IEnumerable<KeyValue>? RequestHeaders { get; set; }

    [CliOption("--method")]
    public string? Method { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}