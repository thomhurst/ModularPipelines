using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "update-layer")]
public record AwsOpsworksUpdateLayerOptions(
[property: CliOption("--layer-id")] string LayerId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--shortname")]
    public string? Shortname { get; set; }

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