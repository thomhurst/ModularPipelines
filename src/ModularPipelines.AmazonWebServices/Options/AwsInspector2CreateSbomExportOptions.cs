using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector2", "create-sbom-export")]
public record AwsInspector2CreateSbomExportOptions(
[property: CommandSwitch("--report-format")] string ReportFormat,
[property: CommandSwitch("--s3-destination")] string S3Destination
) : AwsOptions
{
    [CommandSwitch("--resource-filter-criteria")]
    public string? ResourceFilterCriteria { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}