using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "update")]
public record AzContainerappUpdateOptions : AzOptions
{
    [CliOption("--args")]
    public string? Args { get; set; }

    [CliOption("--artifact")]
    public string? Artifact { get; set; }

    [CliOption("--bind")]
    public string? Bind { get; set; }

    [CliOption("--command")]
    public string? Command { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--cpu")]
    public string? Cpu { get; set; }

    [CliOption("--customized-keys")]
    public string? CustomizedKeys { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--max-replicas")]
    public string? MaxReplicas { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliOption("--min-replicas")]
    public string? MinReplicas { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--remove-all-env-vars")]
    public bool? RemoveAllEnvVars { get; set; }

    [CliOption("--remove-env-vars")]
    public string? RemoveEnvVars { get; set; }

    [CliOption("--replace-env-vars")]
    public string? ReplaceEnvVars { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--revision-suffix")]
    public string? RevisionSuffix { get; set; }

    [CliOption("--scale-rule-auth")]
    public string? ScaleRuleAuth { get; set; }

    [CliOption("--scale-rule-http-concurrency")]
    public string? ScaleRuleHttpConcurrency { get; set; }

    [CliOption("--scale-rule-metadata")]
    public string? ScaleRuleMetadata { get; set; }

    [CliOption("--scale-rule-name")]
    public string? ScaleRuleName { get; set; }

    [CliOption("--scale-rule-type")]
    public string? ScaleRuleType { get; set; }

    [CliOption("--secret-volume-mount")]
    public string? SecretVolumeMount { get; set; }

    [CliOption("--set-env-vars")]
    public string? SetEnvVars { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--termination-grace-period")]
    public string? TerminationGracePeriod { get; set; }

    [CliOption("--unbind")]
    public string? Unbind { get; set; }

    [CliOption("--workload-profile-name")]
    public string? WorkloadProfileName { get; set; }

    [CliOption("--yaml")]
    public string? Yaml { get; set; }
}