using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sync")]
public record SyncOptions : ChocoOptions
{
    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--package-id")]
    public string? PackageId { get; set; }

    [CommandSwitch("--output-directory")]
    public string? OutputDirectory { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public bool? ForceSelfService { get; set; }
}