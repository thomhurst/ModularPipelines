using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "custom-modules", "sha", "update")]
public record GcloudSccCustomModulesShaUpdateOptions(
[property: CliArgument] string CustomModule
) : GcloudOptions
{
    [CliOption("--custom-config-from-file")]
    public string? CustomConfigFromFile { get; set; }

    [CliOption("--enablement-state")]
    public string? EnablementState { get; set; }

    [CliOption("--update-mask")]
    public string? UpdateMask { get; set; }

    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--project")]
    public new string? Project { get; set; }
}