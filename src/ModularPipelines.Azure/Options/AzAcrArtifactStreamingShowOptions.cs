using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "artifact-streaming", "show")]
public record AzAcrArtifactStreamingShowOptions(
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