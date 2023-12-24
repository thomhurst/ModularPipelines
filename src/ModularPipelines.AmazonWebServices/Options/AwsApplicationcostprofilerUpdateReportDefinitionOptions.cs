using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("applicationcostprofiler", "update-report-definition")]
public record AwsApplicationcostprofilerUpdateReportDefinitionOptions(
[property: CommandSwitch("--report-id")] string ReportId,
[property: CommandSwitch("--report-description")] string ReportDescription,
[property: CommandSwitch("--report-frequency")] string ReportFrequency,
[property: CommandSwitch("--format")] string Format,
[property: CommandSwitch("--destination-s3-location")] string DestinationS3Location
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}