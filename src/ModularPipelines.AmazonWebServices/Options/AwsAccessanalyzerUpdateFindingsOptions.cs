using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("accessanalyzer", "update-findings")]
public record AwsAccessanalyzerUpdateFindingsOptions(
[property: CommandSwitch("--analyzer-arn")] string AnalyzerArn,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--ids")]
    public string[]? Ids { get; set; }

    [CommandSwitch("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}