using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "twin", "create")]
public record AzDtTwinCreateOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--dtmi")] string Dtmi,
[property: CommandSwitch("--twin-id")] string TwinId
) : AzOptions
{
    [BooleanCommandSwitch("--if-none-match")]
    public bool? IfNoneMatch { get; set; }

    [CommandSwitch("--properties")]
    public string? Properties { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}