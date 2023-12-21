using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "update")]
public record AzContainerappUpdateOptions : AzOptions
{
    [CommandSwitch("--args")]
    public string? Args { get; set; }

    [CommandSwitch("--artifact")]
    public string? Artifact { get; set; }

    [CommandSwitch("--bind")]
    public string? Bind { get; set; }

    [CommandSwitch("--command")]
    public string? Command { get; set; }

    [CommandSwitch("--container-name")]
    public string? ContainerName { get; set; }

    [CommandSwitch("--cpu")]
    public string? Cpu { get; set; }

    [CommandSwitch("--customized-keys")]
    public string? CustomizedKeys { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--max-replicas")]
    public string? MaxReplicas { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [CommandSwitch("--min-replicas")]
    public string? MinReplicas { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--remove-all-env-vars")]
    public bool? RemoveAllEnvVars { get; set; }

    [CommandSwitch("--remove-env-vars")]
    public string? RemoveEnvVars { get; set; }

    [CommandSwitch("--replace-env-vars")]
    public string? ReplaceEnvVars { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--revision-suffix")]
    public string? RevisionSuffix { get; set; }

    [CommandSwitch("--scale-rule-auth")]
    public string? ScaleRuleAuth { get; set; }

    [CommandSwitch("--scale-rule-http-concurrency")]
    public string? ScaleRuleHttpConcurrency { get; set; }

    [CommandSwitch("--scale-rule-metadata")]
    public string? ScaleRuleMetadata { get; set; }

    [CommandSwitch("--scale-rule-name")]
    public string? ScaleRuleName { get; set; }

    [CommandSwitch("--scale-rule-type")]
    public string? ScaleRuleType { get; set; }

    [CommandSwitch("--secret-volume-mount")]
    public string? SecretVolumeMount { get; set; }

    [CommandSwitch("--set-env-vars")]
    public string? SetEnvVars { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--termination-grace-period")]
    public string? TerminationGracePeriod { get; set; }

    [CommandSwitch("--unbind")]
    public string? Unbind { get; set; }

    [CommandSwitch("--workload-profile-name")]
    public string? WorkloadProfileName { get; set; }

    [CommandSwitch("--yaml")]
    public string? Yaml { get; set; }
}