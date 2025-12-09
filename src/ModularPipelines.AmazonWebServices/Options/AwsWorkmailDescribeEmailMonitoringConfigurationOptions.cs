using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "describe-email-monitoring-configuration")]
public record AwsWorkmailDescribeEmailMonitoringConfigurationOptions(
[property: CliOption("--organization-id")] string OrganizationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}