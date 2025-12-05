using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support", "describe-communications")]
public record AwsSupportDescribeCommunicationsOptions(
[property: CliOption("--case-id")] string CaseId
) : AwsOptions
{
    [CliOption("--before-time")]
    public string? BeforeTime { get; set; }

    [CliOption("--after-time")]
    public string? AfterTime { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}