using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "import", "bak")]
public record GcloudSqlImportBakOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Uri,
[property: CliOption("--database")] string Database
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--bak-type")]
    public string? BakType { get; set; }

    [CliFlag("--no-recovery")]
    public bool? NoRecovery { get; set; }

    [CliFlag("--recovery-only")]
    public bool? RecoveryOnly { get; set; }

    [CliOption("--stop-at")]
    public string? StopAt { get; set; }

    [CliOption("--stop-at-mark")]
    public string? StopAtMark { get; set; }

    [CliOption("--[no-]striped")]
    public string[]? NoStriped { get; set; }

    [CliOption("--cert-path")]
    public string? CertPath { get; set; }

    [CliOption("--pvk-path")]
    public string? PvkPath { get; set; }

    [CliFlag("--prompt-for-pvk-password")]
    public bool? PromptForPvkPassword { get; set; }

    [CliOption("--pvk-password")]
    public string? PvkPassword { get; set; }
}