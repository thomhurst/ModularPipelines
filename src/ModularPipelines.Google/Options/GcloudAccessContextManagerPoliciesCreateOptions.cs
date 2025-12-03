using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "policies", "create")]
public record GcloudAccessContextManagerPoliciesCreateOptions(
[property: CliOption("--organization")] string Organization,
[property: CliOption("--title")] string Title
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--scopes")]
    public string[]? Scopes { get; set; }
}