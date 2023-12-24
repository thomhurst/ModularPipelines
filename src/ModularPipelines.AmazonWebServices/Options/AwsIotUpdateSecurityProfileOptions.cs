using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-security-profile")]
public record AwsIotUpdateSecurityProfileOptions(
[property: CommandSwitch("--security-profile-name")] string SecurityProfileName
) : AwsOptions
{
    [CommandSwitch("--security-profile-description")]
    public string? SecurityProfileDescription { get; set; }

    [CommandSwitch("--behaviors")]
    public string[]? Behaviors { get; set; }

    [CommandSwitch("--alert-targets")]
    public IEnumerable<KeyValue>? AlertTargets { get; set; }

    [CommandSwitch("--additional-metrics-to-retain")]
    public string[]? AdditionalMetricsToRetain { get; set; }

    [CommandSwitch("--additional-metrics-to-retain-v2")]
    public string[]? AdditionalMetricsToRetainV2 { get; set; }

    [CommandSwitch("--expected-version")]
    public long? ExpectedVersion { get; set; }

    [CommandSwitch("--metrics-export-config")]
    public string? MetricsExportConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}