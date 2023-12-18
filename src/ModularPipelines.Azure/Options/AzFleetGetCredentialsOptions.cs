using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fleet", "get-credentials")]
public record AzFleetGetCredentialsOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [BooleanCommandSwitch("--overwrite-existing")]
    public bool? OverwriteExisting { get; set; }
}