using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "describe-app-block-builder-app-block-associations")]
public record AwsAppstreamDescribeAppBlockBuilderAppBlockAssociationsOptions : AwsOptions
{
    [CliOption("--app-block-arn")]
    public string? AppBlockArn { get; set; }

    [CliOption("--app-block-builder-name")]
    public string? AppBlockBuilderName { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}