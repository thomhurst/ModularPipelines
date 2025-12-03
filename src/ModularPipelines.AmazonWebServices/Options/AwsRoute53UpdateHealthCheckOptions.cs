using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "update-health-check")]
public record AwsRoute53UpdateHealthCheckOptions(
[property: CliOption("--health-check-id")] string HealthCheckId
) : AwsOptions
{
    [CliOption("--health-check-version")]
    public long? HealthCheckVersion { get; set; }

    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--resource-path")]
    public string? ResourcePath { get; set; }

    [CliOption("--fully-qualified-domain-name")]
    public string? FullyQualifiedDomainName { get; set; }

    [CliOption("--search-string")]
    public string? SearchString { get; set; }

    [CliOption("--failure-threshold")]
    public int? FailureThreshold { get; set; }

    [CliOption("--health-threshold")]
    public int? HealthThreshold { get; set; }

    [CliOption("--child-health-checks")]
    public string[]? ChildHealthChecks { get; set; }

    [CliOption("--regions")]
    public string[]? Regions { get; set; }

    [CliOption("--alarm-identifier")]
    public string? AlarmIdentifier { get; set; }

    [CliOption("--insufficient-data-health-status")]
    public string? InsufficientDataHealthStatus { get; set; }

    [CliOption("--reset-elements")]
    public string[]? ResetElements { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}