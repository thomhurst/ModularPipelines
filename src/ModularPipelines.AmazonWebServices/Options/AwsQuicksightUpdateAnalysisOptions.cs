using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-analysis")]
public record AwsQuicksightUpdateAnalysisOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--analysis-id")] string AnalysisId,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--parameters")]
    public string? Parameters { get; set; }

    [CliOption("--source-entity")]
    public string? SourceEntity { get; set; }

    [CliOption("--theme-arn")]
    public string? ThemeArn { get; set; }

    [CliOption("--definition")]
    public string? Definition { get; set; }

    [CliOption("--validation-strategy")]
    public string? ValidationStrategy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}