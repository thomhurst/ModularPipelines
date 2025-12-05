using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "domains", "trusts", "validate-state")]
public record GcloudActiveDirectoryDomainsTrustsValidateStateOptions(
[property: CliArgument] string Domain,
[property: CliOption("--target-domain-name")] string TargetDomainName
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}