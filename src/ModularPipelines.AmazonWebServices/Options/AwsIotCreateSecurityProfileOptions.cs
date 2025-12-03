using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-security-profile")]
public record AwsIotCreateSecurityProfileOptions(
[property: CliOption("--security-profile-name")] string SecurityProfileName
) : AwsOptions
{
    [CliOption("--security-profile-description")]
    public string? SecurityProfileDescription { get; set; }

    [CliOption("--behaviors")]
    public string[]? Behaviors { get; set; }

    [CliOption("--alert-targets")]
    public IEnumerable<KeyValue>? AlertTargets { get; set; }

    [CliOption("--additional-metrics-to-retain")]
    public string[]? AdditionalMetricsToRetain { get; set; }

    [CliOption("--additional-metrics-to-retain-v2")]
    public string[]? AdditionalMetricsToRetainV2 { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--metrics-export-config")]
    public string? MetricsExportConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}