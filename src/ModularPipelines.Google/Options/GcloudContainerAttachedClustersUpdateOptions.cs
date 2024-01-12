using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "attached", "clusters", "update")]
public record GcloudContainerAttachedClustersUpdateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--annotations")]
    public string[]? Annotations { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [BooleanCommandSwitch("--clear-description")]
    public bool? ClearDescription { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--logging")]
    public string[]? Logging { get; set; }

    [CommandSwitch("--platform-version")]
    public string? PlatformVersion { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--admin-groups")]
    public string[]? AdminGroups { get; set; }

    [BooleanCommandSwitch("--clear-admin-groups")]
    public bool? ClearAdminGroups { get; set; }

    [CommandSwitch("--admin-users")]
    public string[]? AdminUsers { get; set; }

    [BooleanCommandSwitch("--clear-admin-users")]
    public bool? ClearAdminUsers { get; set; }

    [BooleanCommandSwitch("--disable-managed-prometheus")]
    public bool? DisableManagedPrometheus { get; set; }

    [BooleanCommandSwitch("--enable-managed-prometheus")]
    public bool? EnableManagedPrometheus { get; set; }

    [CommandSwitch("--proxy-secret-name")]
    public string? ProxySecretName { get; set; }

    [CommandSwitch("--proxy-secret-namespace")]
    public string? ProxySecretNamespace { get; set; }
}