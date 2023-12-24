using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "update-layer")]
public record AwsOpsworksUpdateLayerOptions(
[property: CommandSwitch("--layer-id")] string LayerId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--shortname")]
    public string? Shortname { get; set; }

    [CommandSwitch("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CommandSwitch("--cloud-watch-logs-configuration")]
    public string? CloudWatchLogsConfiguration { get; set; }

    [CommandSwitch("--custom-instance-profile-arn")]
    public string? CustomInstanceProfileArn { get; set; }

    [CommandSwitch("--custom-json")]
    public string? CustomJson { get; set; }

    [CommandSwitch("--custom-security-group-ids")]
    public string[]? CustomSecurityGroupIds { get; set; }

    [CommandSwitch("--packages")]
    public string[]? Packages { get; set; }

    [CommandSwitch("--volume-configurations")]
    public string[]? VolumeConfigurations { get; set; }

    [CommandSwitch("--custom-recipes")]
    public string? CustomRecipes { get; set; }

    [CommandSwitch("--lifecycle-event-configuration")]
    public string? LifecycleEventConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}