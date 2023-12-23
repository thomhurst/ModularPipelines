using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cur", "modify-report-definition")]
public record AwsCurModifyReportDefinitionOptions(
[property: CommandSwitch("--report-name")] string ReportName,
[property: CommandSwitch("--report-definition")] string ReportDefinition
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}