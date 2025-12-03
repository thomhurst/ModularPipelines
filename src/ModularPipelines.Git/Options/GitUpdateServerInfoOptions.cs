using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("update-server-info")]
[ExcludeFromCodeCoverage]
public record GitUpdateServerInfoOptions : GitOptions
{
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }
}