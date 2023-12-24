using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-email", "get-deliverability-test-report")]
public record AwsPinpointEmailGetDeliverabilityTestReportOptions(
[property: CommandSwitch("--report-id")] string ReportId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}