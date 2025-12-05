using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "hp-tuning-jobs", "create")]
public record GcloudAiHpTuningJobsCreateOptions(
[property: CliOption("--config")] string Config,
[property: CliOption("--display-name")] string DisplayName
) : GcloudOptions
{
    [CliOption("--algorithm")]
    public string? Algorithm { get; set; }

    [CliFlag("--enable-dashboard-access")]
    public bool? EnableDashboardAccess { get; set; }

    [CliFlag("--enable-web-access")]
    public bool? EnableWebAccess { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--max-trial-count")]
    public string? MaxTrialCount { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--parallel-trial-count")]
    public string? ParallelTrialCount { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-location")]
    public string? KmsLocation { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }
}