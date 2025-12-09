using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("polly", "describe-voices")]
public record AwsPollyDescribeVoicesOptions : AwsOptions
{
    [CliOption("--engine")]
    public string? Engine { get; set; }

    [CliOption("--language-code")]
    public string? LanguageCode { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}