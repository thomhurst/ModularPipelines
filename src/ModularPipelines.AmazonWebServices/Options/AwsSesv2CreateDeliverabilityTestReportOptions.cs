using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "create-deliverability-test-report")]
public record AwsSesv2CreateDeliverabilityTestReportOptions(
[property: CommandSwitch("--from-email-address")] string FromEmailAddress,
[property: CommandSwitch("--content")] string Content
) : AwsOptions
{
    [CommandSwitch("--report-name")]
    public string? ReportName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}