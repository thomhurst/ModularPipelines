using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "put-email-monitoring-configuration")]
public record AwsWorkmailPutEmailMonitoringConfigurationOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--log-group-arn")] string LogGroupArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}