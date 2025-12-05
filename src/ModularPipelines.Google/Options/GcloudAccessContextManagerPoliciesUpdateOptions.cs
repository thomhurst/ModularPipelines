using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "policies", "update")]
public record GcloudAccessContextManagerPoliciesUpdateOptions(
[property: CliArgument] string Policy
) : GcloudOptions
{
    [CliOption("--title")]
    public string? Title { get; set; }
}