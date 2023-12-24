using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("accessanalyzer", "list-access-preview-findings")]
public record AwsAccessanalyzerListAccessPreviewFindingsOptions(
[property: CommandSwitch("--access-preview-id")] string AccessPreviewId,
[property: CommandSwitch("--analyzer-arn")] string AnalyzerArn
) : AwsOptions
{
    [CommandSwitch("--filter")]
    public IEnumerable<KeyValue>? Filter { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}