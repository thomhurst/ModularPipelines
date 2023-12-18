using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "artifact-streaming", "update")]
public record AzAcrArtifactStreamingUpdateOptions(
[property: BooleanCommandSwitch("--enable-streaming")] bool EnableStreaming,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--repository")] string Repository
) : AzOptions
{
    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--suffix")]
    public string? Suffix { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}