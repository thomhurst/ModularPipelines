using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "helm", "install-cli")]
public record AzAcrHelmInstallCliOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--client-version")]
    public string? ClientVersion { get; set; }

    [CommandSwitch("--install-location")]
    public string? InstallLocation { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

