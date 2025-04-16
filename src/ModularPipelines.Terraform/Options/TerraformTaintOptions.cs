using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("taint")]
[ExcludeFromCodeCoverage]
public record TerraformTaintOptions : TerraformOptions
{
    [BooleanCommandSwitch("-replace")]
    public virtual bool? Replace { get; set; }

    [BooleanCommandSwitch("-allow-missing")]
    public virtual bool? AllowMissing { get; set; }

    [BooleanCommandSwitch("-lock")]
    public virtual bool? Lock { get; set; }

    [CommandSwitch("-lock-timeout")]
    public virtual string? LockTimeout { get; set; }

    [BooleanCommandSwitch("-ignore-remote-version")]
    public virtual bool? IgnoreRemoteVersion { get; set; }

    [BooleanCommandSwitch("-state")]
    public virtual bool? State { get; set; }

    [BooleanCommandSwitch("-state-out")]
    public virtual bool? StateOut { get; set; }

    [BooleanCommandSwitch("-backup")]
    public virtual bool? Backup { get; set; }
}