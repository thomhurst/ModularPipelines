using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lakeformation", "list-lake-formation-opt-ins")]
public record AwsLakeformationListLakeFormationOptInsOptions : AwsOptions
{
    [CliOption("--principal")]
    public string? Principal { get; set; }

    [CliOption("--resource")]
    public string? Resource { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}