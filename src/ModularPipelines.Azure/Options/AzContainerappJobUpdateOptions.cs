using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "job", "update")]
public record AzContainerappJobUpdateOptions : AzOptions
{
    [CommandSwitch("--args")]
    public string? Args { get; set; }

    [CommandSwitch("--command")]
    public string? Command { get; set; }

    [CommandSwitch("--container-name")]
    public string? ContainerName { get; set; }

    [CommandSwitch("--cpu")]
    public string? Cpu { get; set; }

    [CommandSwitch("--cron-expression")]
    public string? CronExpression { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--max-executions")]
    public string? MaxExecutions { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [CommandSwitch("--min-executions")]
    public string? MinExecutions { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--parallelism")]
    public string? Parallelism { get; set; }

    [CommandSwitch("--polling-interval")]
    public string? PollingInterval { get; set; }

    [CommandSwitch("--rcc")]
    public string? Rcc { get; set; }

    [BooleanCommandSwitch("--remove-all-env-vars")]
    public bool? RemoveAllEnvVars { get; set; }

    [CommandSwitch("--remove-env-vars")]
    public string? RemoveEnvVars { get; set; }

    [CommandSwitch("--replace-env-vars")]
    public string? ReplaceEnvVars { get; set; }

    [CommandSwitch("--replica-retry-limit")]
    public string? ReplicaRetryLimit { get; set; }

    [CommandSwitch("--replica-timeout")]
    public string? ReplicaTimeout { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--scale-rule-auth")]
    public string? ScaleRuleAuth { get; set; }

    [CommandSwitch("--scale-rule-metadata")]
    public string? ScaleRuleMetadata { get; set; }

    [CommandSwitch("--scale-rule-name")]
    public string? ScaleRuleName { get; set; }

    [CommandSwitch("--scale-rule-type")]
    public string? ScaleRuleType { get; set; }

    [CommandSwitch("--set-env-vars")]
    public string? SetEnvVars { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--workload-profile-name")]
    public string? WorkloadProfileName { get; set; }

    [CommandSwitch("--yaml")]
    public string? Yaml { get; set; }
}

