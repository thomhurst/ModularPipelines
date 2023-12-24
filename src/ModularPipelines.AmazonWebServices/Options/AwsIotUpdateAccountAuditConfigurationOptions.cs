using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-account-audit-configuration")]
public record AwsIotUpdateAccountAuditConfigurationOptions : AwsOptions
{
    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--audit-notification-target-configurations")]
    public IEnumerable<KeyValue>? AuditNotificationTargetConfigurations { get; set; }

    [CommandSwitch("--audit-check-configurations")]
    public IEnumerable<KeyValue>? AuditCheckConfigurations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}