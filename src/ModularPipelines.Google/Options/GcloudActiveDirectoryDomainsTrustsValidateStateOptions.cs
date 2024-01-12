using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "domains", "trusts", "validate-state")]
public record GcloudActiveDirectoryDomainsTrustsValidateStateOptions(
[property: PositionalArgument] string Domain,
[property: CommandSwitch("--target-domain-name")] string TargetDomainName
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}