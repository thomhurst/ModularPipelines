using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support", "describe-cases")]
public record AwsSupportDescribeCasesOptions : AwsOptions
{
    [CliOption("--case-id-list")]
    public string[]? CaseIdList { get; set; }

    [CliOption("--display-id")]
    public string? DisplayId { get; set; }

    [CliOption("--after-time")]
    public string? AfterTime { get; set; }

    [CliOption("--before-time")]
    public string? BeforeTime { get; set; }

    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}