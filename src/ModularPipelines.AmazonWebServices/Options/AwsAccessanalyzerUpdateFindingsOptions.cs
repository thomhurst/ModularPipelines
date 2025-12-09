using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("accessanalyzer", "update-findings")]
public record AwsAccessanalyzerUpdateFindingsOptions(
[property: CliOption("--analyzer-arn")] string AnalyzerArn,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--ids")]
    public string[]? Ids { get; set; }

    [CliOption("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}