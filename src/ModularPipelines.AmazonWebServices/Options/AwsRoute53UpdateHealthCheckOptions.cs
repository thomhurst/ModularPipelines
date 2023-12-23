using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "update-health-check")]
public record AwsRoute53UpdateHealthCheckOptions(
[property: CommandSwitch("--health-check-id")] string HealthCheckId
) : AwsOptions
{
    [CommandSwitch("--health-check-version")]
    public long? HealthCheckVersion { get; set; }

    [CommandSwitch("--ip-address")]
    public string? IpAddress { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--resource-path")]
    public string? ResourcePath { get; set; }

    [CommandSwitch("--fully-qualified-domain-name")]
    public string? FullyQualifiedDomainName { get; set; }

    [CommandSwitch("--search-string")]
    public string? SearchString { get; set; }

    [CommandSwitch("--failure-threshold")]
    public int? FailureThreshold { get; set; }

    [CommandSwitch("--health-threshold")]
    public int? HealthThreshold { get; set; }

    [CommandSwitch("--child-health-checks")]
    public string[]? ChildHealthChecks { get; set; }

    [CommandSwitch("--regions")]
    public string[]? Regions { get; set; }

    [CommandSwitch("--alarm-identifier")]
    public string? AlarmIdentifier { get; set; }

    [CommandSwitch("--insufficient-data-health-status")]
    public string? InsufficientDataHealthStatus { get; set; }

    [CommandSwitch("--reset-elements")]
    public string[]? ResetElements { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}