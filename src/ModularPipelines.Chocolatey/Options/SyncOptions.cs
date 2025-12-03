using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sync")]
public record SyncOptions : ChocoOptions
{
    [CliOption("--id")]
    public virtual string? Id { get; set; }

    [CliOption("--package-id")]
    public virtual string? PackageId { get; set; }

    [CliOption("--output-directory")]
    public virtual string? OutputDirectory { get; set; }

    [CliFlag("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}