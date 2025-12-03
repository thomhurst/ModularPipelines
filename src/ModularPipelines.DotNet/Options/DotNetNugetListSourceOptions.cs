using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CliCommand("nuget", "list", "source")]
[ExcludeFromCodeCoverage]
public record DotNetNugetListSourceOptions : DotNetOptions
{
    [CliFlag("--format")]
    public virtual bool? Format { get; set; }

    [CliOption("--configfile")]
    public virtual string? Configfile { get; set; }
}
