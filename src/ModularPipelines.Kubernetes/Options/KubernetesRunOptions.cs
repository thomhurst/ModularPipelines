using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesRunOptions : KubernetesOptions
{
    public KubernetesRunOptions(
        string image
)
    {
        CommandParts = ["run"];
        Image = image;
    }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandSwitch("--annotations")]
    public IEnumerable<string>? Annotations { get; set; }

    [BooleanCommandSwitch("--attach")]
    public bool? Attach { get; set; }

    [CommandSwitch("--cascade")]
    public string? Cascade { get; set; }

    [BooleanCommandSwitch("--command")]
    public bool? Command { get; set; }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [CommandSwitch("--env")]
    public IEnumerable<string>? Env { get; set; }

    [BooleanCommandSwitch("--expose")]
    public bool? Expose { get; set; }

    [CommandSwitch("--field-manager")]
    public string? FieldManager { get; set; }

    [CommandSwitch("--filename")]
    public IEnumerable<string>? Filename { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--grace-period")]
    public int? GracePeriod { get; set; }

    [CommandSwitch("--hostport")]
    public int? Hostport { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--image-pull-policy")]
    public string? ImagePullPolicy { get; set; }

    [CommandSwitch("--kustomize")]
    public string? Kustomize { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [BooleanCommandSwitch("--leave-stdin-open")]
    public bool? LeaveStdinOpen { get; set; }

    [CommandSwitch("--limits")]
    public string? Limits { get; set; }

    [PositionalArgument(PlaceholderName = "<NAME>")]
    public string? NAME { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--overrides")]
    public string? Overrides { get; set; }

    [CommandSwitch("--pod-running-timeout")]
    public string? PodRunningTimeout { get; set; }

    [CommandSwitch("--port")]
    public string? Port { get; set; }

    [BooleanCommandSwitch("--privileged")]
    public bool? Privileged { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--record")]
    public bool? Record { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [CommandSwitch("--requests")]
    public string? Requests { get; set; }

    [CommandSwitch("--restart")]
    public string? Restart { get; set; }

    [BooleanCommandSwitch("--rm")]
    public bool? Rm { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public bool? SaveConfig { get; set; }

    [CommandSwitch("--serviceaccount")]
    public string? Serviceaccount { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public bool? Stdin { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--tty")]
    public bool? Tty { get; set; }

    [BooleanCommandSwitch("--wait")]
    public bool? Wait { get; set; }
}
