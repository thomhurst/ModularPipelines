using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sts", "get-session-token")]
public record AwsStsGetSessionTokenOptions : AwsOptions
{
    [CliOption("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CliOption("--serial-number")]
    public string? SerialNumber { get; set; }

    [CliOption("--token-code")]
    public string? TokenCode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}