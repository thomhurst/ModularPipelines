using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "muteconfigs", "update")]
public record GcloudSccMuteconfigsUpdateOptions(
[property: CliArgument] string MuteConfig
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--update-mask")]
    public string? UpdateMask { get; set; }

    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--project")]
    public new string? Project { get; set; }
}