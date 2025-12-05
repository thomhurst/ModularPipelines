using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lakeformation", "create-lake-formation-opt-in")]
public record AwsLakeformationCreateLakeFormationOptInOptions(
[property: CliOption("--principal")] string Principal,
[property: CliOption("--resource")] string Resource
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}