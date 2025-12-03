using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("image", "builder", "customizer", "add")]
public record AzImageBuilderCustomizerAddOptions(
[property: CliOption("--customizer-name")] string CustomizerName,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliOption("--defer")]
    public string? Defer { get; set; }

    [CliOption("--dest-path")]
    public string? DestPath { get; set; }

    [CliOption("--exit-codes")]
    public string? ExitCodes { get; set; }

    [CliOption("--file-source")]
    public string? FileSource { get; set; }

    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--inline-script")]
    public string? InlineScript { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--restart-check-command")]
    public string? RestartCheckCommand { get; set; }

    [CliOption("--restart-command")]
    public string? RestartCommand { get; set; }

    [CliOption("--restart-timeout")]
    public string? RestartTimeout { get; set; }

    [CliOption("--script-url")]
    public string? ScriptUrl { get; set; }

    [CliOption("--search-criteria")]
    public string? SearchCriteria { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--update-limit")]
    public string? UpdateLimit { get; set; }
}