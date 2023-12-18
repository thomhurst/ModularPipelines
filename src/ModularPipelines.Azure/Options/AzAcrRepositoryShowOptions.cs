using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "repository", "show")]
public record AzAcrRepositoryShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }

    [CommandSwitch("--suffix")]
    public string? Suffix { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}