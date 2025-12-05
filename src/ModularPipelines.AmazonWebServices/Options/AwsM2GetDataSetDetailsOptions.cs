using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("m2", "get-data-set-details")]
public record AwsM2GetDataSetDetailsOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--data-set-name")] string DataSetName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}