using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("npm", "info")]
public record YarnNpmInfoOptions : YarnOptions
{
    [CliOption("--fields")]
    public virtual string? Fields { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }
}