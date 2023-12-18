using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "repository", "show-manifests")]
public record AzAcrRepositoryShowManifestsOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--repository")] string Repository
) : AzOptions
{
    [BooleanCommandSwitch("--detail")]
    public bool? Detail { get; set; }

    [CommandSwitch("--orderby")]
    public string? Orderby { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--suffix")]
    public string? Suffix { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}