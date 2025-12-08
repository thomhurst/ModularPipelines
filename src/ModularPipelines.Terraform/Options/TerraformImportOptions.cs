using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Terraform.Options;

[CliSubCommand("import")]
[ExcludeFromCodeCoverage]
public record TerraformImportOptions([property: CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    string Address, [property: CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    string Id) : TerraformOptions
{
    [CliOption("-config")]
    public virtual string? Config { get; set; }

    [CliFlag("-input")]
    public virtual bool? Input { get; set; }

    [CliFlag("-lock")]
    public virtual bool? Lock { get; set; }

    [CliOption("-lock-timeout")]
    public virtual string? LockTimeout { get; set; }

    [CliFlag("-no-color")]
    public virtual bool? NoColor { get; set; }

    [CliOption("-parallelism")]
    public virtual int? Parallelism { get; set; }

    [CliOption("-provider")]
    public virtual string? Provider { get; set; }

    [CliOption("-var")]
    public virtual IEnumerable<KeyValue>? Vars { get; set; }

    [CliOption("-var-file")]
    public virtual string? VarFile { get; set; }

    [CliFlag("-ignore-remote-version")]
    public virtual bool? IgnoreRemoteVersion { get; set; }

    [CliFlag("-state")]
    public virtual bool? State { get; set; }

    [CliFlag("-state-out")]
    public virtual bool? StateOut { get; set; }

    [CliFlag("-backup")]
    public virtual bool? Backup { get; set; }
}