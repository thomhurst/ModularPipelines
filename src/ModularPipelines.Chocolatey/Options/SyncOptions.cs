using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sync")]
public record SyncOptions : ChocoOptions
{
    [CommandSwitch("--id")]
    public virtual string? Id { get; set; }

    [CommandSwitch("--package-id")]
    public virtual string? PackageId { get; set; }

    [CommandSwitch("--output-directory")]
    public virtual string? OutputDirectory { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}