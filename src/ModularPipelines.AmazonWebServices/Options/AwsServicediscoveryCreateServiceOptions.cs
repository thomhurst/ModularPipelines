using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicediscovery", "create-service")]
public record AwsServicediscoveryCreateServiceOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--namespace-id")]
    public string? NamespaceId { get; set; }

    [CliOption("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--dns-config")]
    public string? DnsConfig { get; set; }

    [CliOption("--health-check-config")]
    public string? HealthCheckConfig { get; set; }

    [CliOption("--health-check-custom-config")]
    public string? HealthCheckCustomConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}