using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "create-deliverability-test-report")]
public record AwsSesv2CreateDeliverabilityTestReportOptions(
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