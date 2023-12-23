using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataexchange", "send-api-asset")]
public record AwsDataexchangeSendApiAssetOptions(
[property: CommandSwitch("--asset-id")] string AssetId,
[property: CommandSwitch("--data-set-id")] string DataSetId,
[property: CommandSwitch("--revision-id")] string RevisionId
) : AwsOptions
{
    [CommandSwitch("--body")]
    public string? Body { get; set; }

    [CommandSwitch("--query-string-parameters")]
    public IEnumerable<KeyValue>? QueryStringParameters { get; set; }

    [CommandSwitch("--request-headers")]
    public IEnumerable<KeyValue>? RequestHeaders { get; set; }

    [CommandSwitch("--method")]
    public string? Method { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}