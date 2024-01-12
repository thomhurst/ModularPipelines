using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "hp-tuning-jobs", "create")]
public record GcloudAiHpTuningJobsCreateOptions(
[property: CommandSwitch("--config")] string Config,
[property: CommandSwitch("--display-name")] string DisplayName
) : GcloudOptions
{
    [CommandSwitch("--algorithm")]
    public string? Algorithm { get; set; }

    [BooleanCommandSwitch("--enable-dashboard-access")]
    public bool? EnableDashboardAccess { get; set; }

    [BooleanCommandSwitch("--enable-web-access")]
    public bool? EnableWebAccess { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--max-trial-count")]
    public string? MaxTrialCount { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--parallel-trial-count")]
    public string? ParallelTrialCount { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }
}