using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("k8sconfiguration", "create")]
public record AzK8sconfigurationCreateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--cluster-type")] string ClusterType,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--repository-url")] string RepositoryUrl,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--scope")] string Scope
) : AzOptions
{
    [BooleanCommandSwitch("--enable-helm-operator")]
    public bool? EnableHelmOperator { get; set; }

    [CommandSwitch("--helm-operator-params")]
    public string? HelmOperatorParams { get; set; }

    [CommandSwitch("--helm-operator-version")]
    public string? HelmOperatorVersion { get; set; }

    [CommandSwitch("--https-key")]
    public string? HttpsKey { get; set; }

    [CommandSwitch("--https-user")]
    public string? HttpsUser { get; set; }

    [CommandSwitch("--operator-instance-name")]
    public string? OperatorInstanceName { get; set; }

    [CommandSwitch("--operator-namespace")]
    public string? OperatorNamespace { get; set; }

    [CommandSwitch("--operator-params")]
    public string? OperatorParams { get; set; }

    [CommandSwitch("--operator-type")]
    public string? OperatorType { get; set; }

    [CommandSwitch("--ssh-known-hosts")]
    public string? SshKnownHosts { get; set; }

    [CommandSwitch("--ssh-known-hosts-file")]
    public string? SshKnownHostsFile { get; set; }

    [CommandSwitch("--ssh-private-key")]
    public string? SshPrivateKey { get; set; }

    [CommandSwitch("--ssh-private-key-file")]
    public string? SshPrivateKeyFile { get; set; }
}

