using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "job", "update")]
public record AzContainerappJobUpdateOptions : AzOptions
{
    [CliOption("--args")]
    public string? Args { get; set; }

    [CliOption("--command")]
    public string? Command { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--cpu")]
    public string? Cpu { get; set; }

    [CliOption("--cron-expression")]
    public string? CronExpression { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--max-executions")]
    public string? MaxExecutions { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliOption("--min-executions")]
    public string? MinExecutions { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--parallelism")]
    public string? Parallelism { get; set; }

    [CliOption("--polling-interval")]
    public string? PollingInterval { get; set; }

    [CliOption("--rcc")]
    public string? Rcc { get; set; }

    [CliFlag("--remove-all-env-vars")]
    public bool? RemoveAllEnvVars { get; set; }

    [CliOption("--remove-env-vars")]
    public string? RemoveEnvVars { get; set; }

    [CliOption("--replace-env-vars")]
    public string? ReplaceEnvVars { get; set; }

    [CliOption("--replica-retry-limit")]
    public string? ReplicaRetryLimit { get; set; }

    [CliOption("--replica-timeout")]
    public string? ReplicaTimeout { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scale-rule-auth")]
    public string? ScaleRuleAuth { get; set; }

    [CliOption("--scale-rule-metadata")]
    public string? ScaleRuleMetadata { get; set; }

    [CliOption("--scale-rule-name")]
    public string? ScaleRuleName { get; set; }

    [CliOption("--scale-rule-type")]
    public string? ScaleRuleType { get; set; }

    [CliOption("--set-env-vars")]
    public string? SetEnvVars { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--workload-profile-name")]
    public string? WorkloadProfileName { get; set; }

    [CliOption("--yaml")]
    public string? Yaml { get; set; }
}