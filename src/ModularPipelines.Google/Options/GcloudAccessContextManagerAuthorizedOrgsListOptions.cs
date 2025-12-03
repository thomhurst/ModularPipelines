using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "authorized-orgs", "list")]
public record GcloudAccessContextManagerAuthorizedOrgsListOptions : GcloudOptions
{
    [CliOption("--policy")]
    public string? Policy { get; set; }
}