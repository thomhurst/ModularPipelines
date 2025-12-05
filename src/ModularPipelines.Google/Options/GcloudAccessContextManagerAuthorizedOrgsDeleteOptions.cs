using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "authorized-orgs", "delete")]
public record GcloudAccessContextManagerAuthorizedOrgsDeleteOptions(
[property: CliArgument] string AuthorizedOrgsDesc,
[property: CliArgument] string Policy
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}