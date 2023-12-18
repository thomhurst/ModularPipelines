using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "twin", "update")]
public record AzDtTwinUpdateOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--json-patch")] string JsonPatch,
[property: CommandSwitch("--twin-id")] string TwinId
) : AzOptions
{
    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}