using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("run")]
[ExcludeFromCodeCoverage]
public record KubernetesRunOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--annotations")]
    public string[]? Annotations { get; set; }

    [CliFlag("--attach")]
    public virtual bool? Attach { get; set; }

    [CliOption("--cascade")]
    public string? Cascade { get; set; }

    [CliFlag("--command")]
    public virtual bool? Command { get; set; }

    [CliOption("--dry-run")]
    public string? DryRun { get; set; }

    [CliOption("--env")]
    public string[]? Env { get; set; }

    [CliFlag("--expose")]
    public virtual bool? Expose { get; set; }

    [CliOption("--field-manager")]
    public string? FieldManager { get; set; }

    [CliOption("--filename")]
    public string[]? Filename { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--grace-period")]
    public int? GracePeriod { get; set; }

    [CliOption("--hostport")]
    public int? Hostport { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--image-pull-policy")]
    public string? ImagePullPolicy { get; set; }

    [CliOption("--kustomize")]
    public string? Kustomize { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliFlag("--leave-stdin-open")]
    public virtual bool? LeaveStdinOpen { get; set; }

    [CliOption("--limits")]
    public string? Limits { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliOption("--overrides")]
    public string? Overrides { get; set; }

    [CliOption("--pod-running-timeout")]
    public string? PodRunningTimeout { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliFlag("--privileged")]
    public virtual bool? Privileged { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--record")]
    public virtual bool? Record { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliOption("--requests")]
    public string? Requests { get; set; }

    [CliOption("--restart")]
    public string? Restart { get; set; }

    [CliFlag("--rm")]
    public virtual bool? Rm { get; set; }

    [CliFlag("--save-config")]
    public virtual bool? SaveConfig { get; set; }

    [CliOption("--serviceaccount")]
    public string? Serviceaccount { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliFlag("--stdin")]
    public virtual bool? Stdin { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliFlag("--tty")]
    public virtual bool? Tty { get; set; }

    [CliFlag("--wait")]
    public virtual bool? Wait { get; set; }
}