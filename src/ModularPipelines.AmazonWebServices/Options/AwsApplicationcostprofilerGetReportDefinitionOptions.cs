using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("applicationcostprofiler", "get-report-definition")]
public record AwsApplicationcostprofilerGetReportDefinitionOptions(
[property: CommandSwitch("--report-id")] string ReportId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}