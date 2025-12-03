using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lakeformation", "delete-lake-formation-opt-in")]
public record AwsLakeformationDeleteLakeFormationOptInOptions(
[property: CliOption("--principal")] string Principal,
[property: CliOption("--resource")] string Resource
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}