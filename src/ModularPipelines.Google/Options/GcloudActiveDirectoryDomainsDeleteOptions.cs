using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "domains", "delete")]
public record GcloudActiveDirectoryDomainsDeleteOptions(
[property: CliArgument] string Domain
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}