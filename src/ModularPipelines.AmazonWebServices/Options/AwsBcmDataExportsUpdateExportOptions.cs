using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bcm-data-exports", "update-export")]
public record AwsBcmDataExportsUpdateExportOptions(
[property: CliOption("--export")] string Export,
[property: CliOption("--export-arn")] string ExportArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}