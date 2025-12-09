using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support", "describe-services")]
public record AwsSupportDescribeServicesOptions : AwsOptions
{
    [CliOption("--service-code-list")]
    public string[]? ServiceCodeList { get; set; }

    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}