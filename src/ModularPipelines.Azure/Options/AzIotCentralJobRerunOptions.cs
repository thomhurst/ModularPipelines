using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "job", "rerun")]
public record AzIotCentralJobRerunOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--rerun-id")] string RerunId
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}