using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-email", "create-deliverability-test-report")]
public record AwsPinpointEmailCreateDeliverabilityTestReportOptions(
[property: CliOption("--from-email-address")] string FromEmailAddress,
[property: CliOption("--content")] string Content
) : AwsOptions
{
    [CliOption("--report-name")]
    public string? ReportName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}