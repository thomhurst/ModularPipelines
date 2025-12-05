using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "describe-event-categories")]
public record AwsRedshiftDescribeEventCategoriesOptions : AwsOptions
{
    [CliOption("--source-type")]
    public string? SourceType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}