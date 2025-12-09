using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "create-kx-cluster")]
public record AwsFinspaceCreateKxClusterOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--cluster-type")] string ClusterType,
[property: CliOption("--release-label")] string ReleaseLabel,
[property: CliOption("--vpc-configuration")] string VpcConfiguration,
[property: CliOption("--az-mode")] string AzMode
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tickerplant-log-configuration")]
    public string? TickerplantLogConfiguration { get; set; }

    [CliOption("--databases")]
    public string[]? Databases { get; set; }

    [CliOption("--cache-storage-configurations")]
    public string[]? CacheStorageConfigurations { get; set; }

    [CliOption("--auto-scaling-configuration")]
    public string? AutoScalingConfiguration { get; set; }

    [CliOption("--cluster-description")]
    public string? ClusterDescription { get; set; }

    [CliOption("--capacity-configuration")]
    public string? CapacityConfiguration { get; set; }

    [CliOption("--initialization-script")]
    public string? InitializationScript { get; set; }

    [CliOption("--command-line-arguments")]
    public string[]? CommandLineArguments { get; set; }

    [CliOption("--code")]
    public string? Code { get; set; }

    [CliOption("--execution-role")]
    public string? ExecutionRole { get; set; }

    [CliOption("--savedown-storage-configuration")]
    public string? SavedownStorageConfiguration { get; set; }

    [CliOption("--availability-zone-id")]
    public string? AvailabilityZoneId { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--scaling-group-configuration")]
    public string? ScalingGroupConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}