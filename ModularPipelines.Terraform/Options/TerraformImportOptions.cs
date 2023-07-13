using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("import")]
public record TerraformImportOptions([property: PositionalArgument(Position = Position.AfterArguments)]
    string Address, [property: PositionalArgument(Position = Position.AfterArguments)]
    string Id) : TerraformOptions
{
    [CommandSwitch("-config")] public string? Config { get; set; }

    [BooleanCommandSwitch("-input")] public bool? Input { get; set; }

    [BooleanCommandSwitch("-lock")] public bool? Lock { get; set; }

    [CommandSwitch("-lock-timeout")] public string? LockTimeout { get; set; }

    [BooleanCommandSwitch("-no-color")] public bool? NoColor { get; set; }

    [CommandSwitch("-parallelism")] public int? Parallelism { get; set; }

    [CommandSwitch("-provider")] public string? Provider { get; set; }

    [CommandSwitch("-var")] public KeyValueVariables? Vars { get; set; }

    [CommandSwitch("-var-file")] public string? VarFile { get; set; }

    [BooleanCommandSwitch("-ignore-remote-version")]
    public bool? IgnoreRemoteVersion { get; set; }

    [BooleanCommandSwitch("-state")] public bool? State { get; set; }

    [BooleanCommandSwitch("-state-out")] public bool? StateOut { get; set; }

    [BooleanCommandSwitch("-backup")] public bool? Backup { get; set; }
}