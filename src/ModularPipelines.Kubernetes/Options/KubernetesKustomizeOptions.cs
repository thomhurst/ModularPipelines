using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliSubCommand("kustomize")]
[ExcludeFromCodeCoverage]
public record KubernetesKustomizeOptions([property: CliArgument] string Dir) : KubernetesOptions
{
    [CliFlag("--as-current-user")]
    public virtual bool? AsCurrentUser { get; set; }

    [CliFlag("--enable-alpha-plugins")]
    public virtual bool? EnableAlphaPlugins { get; set; }

    [CliFlag("--enable-helm")]
    public virtual bool? EnableHelm { get; set; }

    [CliFlag("--enable-managedby-label")]
    public virtual bool? EnableManagedbyLabel { get; set; }

    [CliOption("--env")]
    public virtual string[]? Env { get; set; }

    [CliOption("--helm-command")]
    public virtual string? HelmCommand { get; set; }

    [CliOption("--load-restrictor")]
    public virtual string? LoadRestrictor { get; set; }

    [CliOption("--mount")]
    public virtual string[]? Mount { get; set; }

    [CliFlag("--network")]
    public virtual bool? Network { get; set; }

    [CliOption("--network-name")]
    public virtual string? NetworkName { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--reorder")]
    public virtual string? Reorder { get; set; }
}