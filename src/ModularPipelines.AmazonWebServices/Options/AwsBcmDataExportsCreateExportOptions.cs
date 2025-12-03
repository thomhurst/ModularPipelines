using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bcm-data-exports", "create-export")]
public record AwsBcmDataExportsCreateExportOptions(
[property: CliOption("--export")] string Export
) : AwsOptions
{
    [CliOption("--resource-tags")]
    public string[]? ResourceTags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}