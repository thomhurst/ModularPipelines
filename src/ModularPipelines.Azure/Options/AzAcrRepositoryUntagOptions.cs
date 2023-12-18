using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "repository", "untag")]
public record AzAcrRepositoryUntagOptions(
[property: CommandSwitch("--image")] string Image,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--suffix")]
    public string? Suffix { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}