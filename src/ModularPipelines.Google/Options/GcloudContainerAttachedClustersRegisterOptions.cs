using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "attached", "clusters", "register")]
public record GcloudContainerAttachedClustersRegisterOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--distribution")] string Distribution,
[property: CommandSwitch("--fleet-project")] string FleetProject,
[property: CommandSwitch("--platform-version")] string PlatformVersion,
[property: CommandSwitch("--context")] string Context,
[property: CommandSwitch("--kubeconfig")] string Kubeconfig,
[property: BooleanCommandSwitch("--has-private-issuer")] bool HasPrivateIssuer,
[property: CommandSwitch("--issuer-url")] string IssuerUrl
) : GcloudOptions
{
    [CommandSwitch("--admin-groups")]
    public string[]? AdminGroups { get; set; }

    [CommandSwitch("--admin-users")]
    public string[]? AdminUsers { get; set; }

    [CommandSwitch("--annotations")]
    public string[]? Annotations { get; set; }

    [CommandSwitch("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-managed-prometheus")]
    public bool? EnableManagedPrometheus { get; set; }

    [CommandSwitch("--logging")]
    public string[]? Logging { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--proxy-secret-name")]
    public string? ProxySecretName { get; set; }

    [CommandSwitch("--proxy-secret-namespace")]
    public string? ProxySecretNamespace { get; set; }
}