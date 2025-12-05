using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dax", "update-cluster")]
public record AwsDaxUpdateClusterOptions(
[property: CliOption("--cluster-name")] string ClusterName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--notification-topic-arn")]
    public string? NotificationTopicArn { get; set; }

    [CliOption("--notification-topic-status")]
    public string? NotificationTopicStatus { get; set; }

    [CliOption("--parameter-group-name")]
    public string? ParameterGroupName { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}