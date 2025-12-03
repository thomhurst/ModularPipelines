using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("list")]
[ExcludeFromCodeCoverage]
public record HelmListOptions : HelmOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--all-namespaces")]
    public virtual bool? AllNamespaces { get; set; }

    [CliFlag("--date")]
    public virtual bool? Date { get; set; }

    [CliFlag("--deployed")]
    public virtual bool? Deployed { get; set; }

    [CliFlag("--failed")]
    public virtual bool? Failed { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--max")]
    public int? Max { get; set; }

    [CliFlag("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CliOption("--offset")]
    public int? Offset { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliFlag("--pending")]
    public virtual bool? Pending { get; set; }

    [CliFlag("--reverse")]
    public virtual bool? Reverse { get; set; }

    [CliOption("--selector")]
    public string? Selector { get; set; }

    [CliFlag("--short")]
    public virtual bool? Short { get; set; }

    [CliFlag("--superseded")]
    public virtual bool? Superseded { get; set; }

    [CliOption("--time-format")]
    public string? TimeFormat { get; set; }

    [CliFlag("--uninstalled")]
    public virtual bool? Uninstalled { get; set; }

    [CliFlag("--uninstalling")]
    public virtual bool? Uninstalling { get; set; }
}