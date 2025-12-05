using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "list-datasets")]
public record AwsComprehendListDatasetsOptions : AwsOptions
{
    [CliOption("--flywheel-arn")]
    public string? FlywheelArn { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}