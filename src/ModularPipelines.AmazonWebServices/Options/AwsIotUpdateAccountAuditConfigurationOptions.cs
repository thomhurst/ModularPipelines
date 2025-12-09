using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-account-audit-configuration")]
public record AwsIotUpdateAccountAuditConfigurationOptions : AwsOptions
{
    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--audit-notification-target-configurations")]
    public IEnumerable<KeyValue>? AuditNotificationTargetConfigurations { get; set; }

    [CliOption("--audit-check-configurations")]
    public IEnumerable<KeyValue>? AuditCheckConfigurations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}