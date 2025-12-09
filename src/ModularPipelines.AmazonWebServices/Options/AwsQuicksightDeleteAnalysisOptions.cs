using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "delete-analysis")]
public record AwsQuicksightDeleteAnalysisOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--analysis-id")] string AnalysisId
) : AwsOptions
{
    [CliOption("--recovery-window-in-days")]
    public long? RecoveryWindowInDays { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}