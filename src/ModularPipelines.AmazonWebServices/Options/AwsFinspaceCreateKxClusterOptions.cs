using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("finspace", "create-kx-cluster")]
public record AwsFinspaceCreateKxClusterOptions(
[property: CommandSwitch("--environment-id")] string EnvironmentId,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--cluster-type")] string ClusterType,
[property: CommandSwitch("--release-label")] string ReleaseLabel,
[property: CommandSwitch("--vpc-configuration")] string VpcConfiguration,
[property: CommandSwitch("--az-mode")] string AzMode
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tickerplant-log-configuration")]
    public string? TickerplantLogConfiguration { get; set; }

    [CommandSwitch("--databases")]
    public string[]? Databases { get; set; }

    [CommandSwitch("--cache-storage-configurations")]
    public string[]? CacheStorageConfigurations { get; set; }

    [CommandSwitch("--auto-scaling-configuration")]
    public string? AutoScalingConfiguration { get; set; }

    [CommandSwitch("--cluster-description")]
    public string? ClusterDescription { get; set; }

    [CommandSwitch("--capacity-configuration")]
    public string? CapacityConfiguration { get; set; }

    [CommandSwitch("--initialization-script")]
    public string? InitializationScript { get; set; }

    [CommandSwitch("--command-line-arguments")]
    public string[]? CommandLineArguments { get; set; }

    [CommandSwitch("--code")]
    public string? Code { get; set; }

    [CommandSwitch("--execution-role")]
    public string? ExecutionRole { get; set; }

    [CommandSwitch("--savedown-storage-configuration")]
    public string? SavedownStorageConfiguration { get; set; }

    [CommandSwitch("--availability-zone-id")]
    public string? AvailabilityZoneId { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--scaling-group-configuration")]
    public string? ScalingGroupConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}