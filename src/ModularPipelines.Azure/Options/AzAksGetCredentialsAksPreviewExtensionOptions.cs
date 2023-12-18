using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "get-credentials", "(aks-preview", "extension)")]
public record AzAksGetCredentialsAksPreviewExtensionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--admin")]
    public bool? Admin { get; set; }

    [CommandSwitch("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [BooleanCommandSwitch("--overwrite-existing")]
    public bool? OverwriteExisting { get; set; }

    [BooleanCommandSwitch("--public-fqdn")]
    public bool? PublicFqdn { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }
}

