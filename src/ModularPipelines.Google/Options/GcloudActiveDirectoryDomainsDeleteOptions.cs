using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "domains", "delete")]
public record GcloudActiveDirectoryDomainsDeleteOptions(
[property: PositionalArgument] string Domain
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}