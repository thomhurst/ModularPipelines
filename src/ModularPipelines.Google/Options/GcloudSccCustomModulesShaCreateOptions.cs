using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "custom-modules", "sha", "create")]
public record GcloudSccCustomModulesShaCreateOptions(
[property: CliOption("--custom-config-from-file")] string CustomConfigFromFile,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--enablement-state")] string EnablementState
) : GcloudOptions
{
    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--project")]
    public new string? Project { get; set; }
}