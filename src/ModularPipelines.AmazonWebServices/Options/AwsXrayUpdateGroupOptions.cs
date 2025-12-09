using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("xray", "update-group")]
public record AwsXrayUpdateGroupOptions : AwsOptions
{
    [CliOption("--group-name")]
    public string? GroupName { get; set; }

    [CliOption("--group-arn")]
    public string? GroupArn { get; set; }

    [CliOption("--filter-expression")]
    public string? FilterExpression { get; set; }

    [CliOption("--insights-configuration")]
    public string? InsightsConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}