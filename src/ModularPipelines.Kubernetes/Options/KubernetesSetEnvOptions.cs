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
    public string? Containers { get; set; }

    [CliOption("--dry-run")]
    public string? DryRun { get; set; }

    [CliOption("--env")]
    public string[]? Env { get; set; }

    [CliOption("--field-manager")]
    public string? FieldManager { get; set; }

    [CliOption("--filename")]
    public string[]? Filename { get; set; }

    [CliOption("--from")]
    public string? From { get; set; }

    [CliOption("--keys")]
    public string[]? Keys { get; set; }

    [CliOption("--kustomize")]
    public string? Kustomize { get; set; }

    [CliFlag("--list")]
    public virtual bool? List { get; set; }

    [CliFlag("--local")]
    public virtual bool? Local { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliFlag("--overwrite")]
    public virtual bool? Overwrite { get; set; }

    [CliOption("--prefix")]
    public string? Prefix { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliFlag("--resolve")]
    public virtual bool? Resolve { get; set; }

    [CliOption("--selector")]
    public string? Selector { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }
}