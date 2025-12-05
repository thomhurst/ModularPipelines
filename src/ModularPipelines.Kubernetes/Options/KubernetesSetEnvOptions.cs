using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("set", "env")]
[ExcludeFromCodeCoverage]
public record KubernetesSetEnvOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--containers")]
    public virtual string? Containers { get; set; }

    [CliOption("--dry-run")]
    public virtual string? DryRun { get; set; }

    [CliOption("--env")]
    public virtual string[]? Env { get; set; }

    [CliOption("--field-manager")]
    public virtual string? FieldManager { get; set; }

    [CliOption("--filename")]
    public virtual string[]? Filename { get; set; }

    [CliOption("--from")]
    public virtual string? From { get; set; }

    [CliOption("--keys")]
    public virtual string[]? Keys { get; set; }

    [CliOption("--kustomize")]
    public virtual string? Kustomize { get; set; }

    [CliFlag("--list")]
    public virtual bool? List { get; set; }

    [CliFlag("--local")]
    public virtual bool? Local { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliFlag("--overwrite")]
    public virtual bool? Overwrite { get; set; }

    [CliOption("--prefix")]
    public virtual string? Prefix { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliFlag("--resolve")]
    public virtual bool? Resolve { get; set; }

    [CliOption("--selector")]
    public virtual string? Selector { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }
}