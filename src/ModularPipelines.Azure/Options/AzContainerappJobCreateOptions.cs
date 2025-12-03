using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "job", "create")]
public record AzContainerappJobCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
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

    [CliOption("--env-vars")]
    public string? EnvVars { get; set; }

    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--environment-type")]
    public string? EnvironmentType { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--max-executions")]
    public string? MaxExecutions { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliFlag("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliOption("--min-executions")]
    public string? MinExecutions { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--parallelism")]
    public string? Parallelism { get; set; }

    [CliOption("--polling-interval")]
    public string? PollingInterval { get; set; }

    [CliOption("--rcc")]
    public string? Rcc { get; set; }

    [CliOption("--registry-identity")]
    public string? RegistryIdentity { get; set; }

    [CliOption("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CliOption("--registry-server")]
    public string? RegistryServer { get; set; }

    [CliOption("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CliOption("--replica-retry-limit")]
    public string? ReplicaRetryLimit { get; set; }

    [CliOption("--replica-timeout")]
    public string? ReplicaTimeout { get; set; }

    [CliOption("--scale-rule-auth")]
    public string? ScaleRuleAuth { get; set; }

    [CliOption("--scale-rule-metadata")]
    public string? ScaleRuleMetadata { get; set; }

    [CliOption("--scale-rule-name")]
    public string? ScaleRuleName { get; set; }

    [CliOption("--scale-rule-type")]
    public string? ScaleRuleType { get; set; }

    [CliOption("--secrets")]
    public string? Secrets { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--trigger-type")]
    public string? TriggerType { get; set; }

    [CliOption("--workload-profile-name")]
    public string? WorkloadProfileName { get; set; }

    [CliOption("--yaml")]
    public string? Yaml { get; set; }
}