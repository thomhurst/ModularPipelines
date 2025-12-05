using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "agents", "delete")]
public record GcloudTransferAgentsDeleteOptions : GcloudOptions
{
    [CliOption("--ids")]
    public string[]? Ids { get; set; }

    [CliFlag("--all")]
    public bool? All { get; set; }

    [CliFlag("--uninstall")]
    public bool? Uninstall { get; set; }
}