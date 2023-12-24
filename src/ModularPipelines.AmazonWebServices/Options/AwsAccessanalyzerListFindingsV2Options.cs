using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("accessanalyzer", "list-findings-v2")]
public record AwsAccessanalyzerListFindingsV2Options(
[property: CommandSwitch("--analyzer-arn")] string AnalyzerArn
) : AwsOptions
{
    [CommandSwitch("--filter")]
    public IEnumerable<KeyValue>? Filter { get; set; }

    [CommandSwitch("--sort")]
    public string? Sort { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}