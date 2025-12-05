using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("honeycode", "get-screen-data")]
public record AwsHoneycodeGetScreenDataOptions(
[property: CliOption("--workbook-id")] string WorkbookId,
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--screen-id")] string ScreenId
) : AwsOptions
{
    [CliOption("--variables")]
    public IEnumerable<KeyValue>? Variables { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}