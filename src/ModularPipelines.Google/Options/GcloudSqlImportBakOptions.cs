using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "import", "bak")]
public record GcloudSqlImportBakOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Uri,
[property: CommandSwitch("--database")] string Database
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--bak-type")]
    public string? BakType { get; set; }

    [BooleanCommandSwitch("--no-recovery")]
    public bool? NoRecovery { get; set; }

    [BooleanCommandSwitch("--recovery-only")]
    public bool? RecoveryOnly { get; set; }

    [CommandSwitch("--stop-at")]
    public string? StopAt { get; set; }

    [CommandSwitch("--stop-at-mark")]
    public string? StopAtMark { get; set; }

    [CommandSwitch("--[no-]striped")]
    public string[]? NoStriped { get; set; }

    [CommandSwitch("--cert-path")]
    public string? CertPath { get; set; }

    [CommandSwitch("--pvk-path")]
    public string? PvkPath { get; set; }

    [BooleanCommandSwitch("--prompt-for-pvk-password")]
    public bool? PromptForPvkPassword { get; set; }

    [CommandSwitch("--pvk-password")]
    public string? PvkPassword { get; set; }
}