using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("wait")]
[ExcludeFromCodeCoverage]
public record KubernetesWaitOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--all-namespaces")]
    public virtual bool? AllNamespaces { get; set; }

    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--field-selector")]
    public virtual string? FieldSelector { get; set; }

    [CliOption("--filename")]
    public virtual string[]? Filename { get; set; }

    [CliOption("--for")]
    public virtual string? For { get; set; }

    [CliFlag("--local")]
    public virtual bool? Local { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliOption("--selector")]
    public virtual string? Selector { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }

    [CliOption("--timeout")]
    public virtual string? Timeout { get; set; }
}