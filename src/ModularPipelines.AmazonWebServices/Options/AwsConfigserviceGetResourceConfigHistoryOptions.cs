using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "get-resource-config-history")]
public record AwsConfigserviceGetResourceConfigHistoryOptions(
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--resource-id")] string ResourceId
) : AwsOptions
{
    [CliOption("--later-time")]
    public long? LaterTime { get; set; }

    [CliOption("--earlier-time")]
    public long? EarlierTime { get; set; }

    [CliOption("--chronological-order")]
    public string? ChronologicalOrder { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}