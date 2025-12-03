using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("accessanalyzer", "list-access-preview-findings")]
public record AwsAccessanalyzerListAccessPreviewFindingsOptions(
[property: CliOption("--access-preview-id")] string AccessPreviewId,
[property: CliOption("--analyzer-arn")] string AnalyzerArn
) : AwsOptions
{
    [CliOption("--filter")]
    public IEnumerable<KeyValue>? Filter { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}