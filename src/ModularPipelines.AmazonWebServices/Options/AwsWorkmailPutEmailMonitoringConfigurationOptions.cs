using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "put-email-monitoring-configuration")]
public record AwsWorkmailPutEmailMonitoringConfigurationOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--log-group-arn")] string LogGroupArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}