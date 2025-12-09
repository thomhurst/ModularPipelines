using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("detective", "update-datasource-packages")]
public record AwsDetectiveUpdateDatasourcePackagesOptions(
[property: CliOption("--graph-arn")] string GraphArn,
[property: CliOption("--datasource-packages")] string[] DatasourcePackages
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}