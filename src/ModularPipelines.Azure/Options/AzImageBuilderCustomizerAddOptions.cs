using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("image", "builder", "customizer", "add")]
public record AzImageBuilderCustomizerAddOptions(
[property: CommandSwitch("--customizer-name")] string CustomizerName,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [CommandSwitch("--defer")]
    public string? Defer { get; set; }

    [CommandSwitch("--dest-path")]
    public string? DestPath { get; set; }

    [CommandSwitch("--exit-codes")]
    public string? ExitCodes { get; set; }

    [CommandSwitch("--file-source")]
    public string? FileSource { get; set; }

    [CommandSwitch("--filters")]
    public string? Filters { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--inline-script")]
    public string? InlineScript { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--restart-check-command")]
    public string? RestartCheckCommand { get; set; }

    [CommandSwitch("--restart-command")]
    public string? RestartCommand { get; set; }

    [CommandSwitch("--restart-timeout")]
    public string? RestartTimeout { get; set; }

    [CommandSwitch("--script-url")]
    public string? ScriptUrl { get; set; }

    [CommandSwitch("--search-criteria")]
    public string? SearchCriteria { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--update-limit")]
    public string? UpdateLimit { get; set; }
}