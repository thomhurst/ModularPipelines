using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "create-layer")]
public record AwsOpsworksCreateLayerOptions(
[property: CliOption("--stack-id")] string StackId,
[property: CliOption("--type")] string Type,
[property: CliOption("--name")] string Name,
[property: CliOption("--shortname")] string Shortname
) : AwsOptions
{
    [CliOption("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CliOption("--cloud-watch-logs-configuration")]
    public string? CloudWatchLogsConfiguration { get; set; }

    [CliOption("--custom-instance-profile-arn")]
    public string? CustomInstanceProfileArn { get; set; }

    [CliOption("--custom-json")]
    public string? CustomJson { get; set; }

    [CliOption("--custom-security-group-ids")]
    public string[]? CustomSecurityGroupIds { get; set; }

    [CliOption("--packages")]
    public string[]? Packages { get; set; }

    [CliOption("--volume-configurations")]
    public string[]? VolumeConfigurations { get; set; }

    [CliOption("--custom-recipes")]
    public string? CustomRecipes { get; set; }

    [CliOption("--lifecycle-event-configuration")]
    public string? LifecycleEventConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}