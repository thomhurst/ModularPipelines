using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "authorized-orgs", "delete")]
public record GcloudAccessContextManagerAuthorizedOrgsDeleteOptions(
[property: PositionalArgument] string AuthorizedOrgsDesc,
[property: PositionalArgument] string Policy
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}