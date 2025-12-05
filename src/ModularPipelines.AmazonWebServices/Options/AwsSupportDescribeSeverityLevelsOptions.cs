using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support", "describe-severity-levels")]
public record AwsSupportDescribeSeverityLevelsOptions : AwsOptions
{
    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}