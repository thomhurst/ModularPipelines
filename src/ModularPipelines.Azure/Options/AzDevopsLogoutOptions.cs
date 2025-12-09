using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "logout")]
public record AzDevopsLogoutOptions : AzOptions
{
    [CliOption("--org")]
    public string? Org { get; set; }
}