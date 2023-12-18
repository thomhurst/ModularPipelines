using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "api-token", "delete")]
public record AzIotCentralApiTokenDeleteOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--tkid")] string Tkid
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}