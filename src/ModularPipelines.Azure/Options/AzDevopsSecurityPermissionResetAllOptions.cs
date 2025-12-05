using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "security", "permission", "reset-all")]
public record AzDevopsSecurityPermissionResetAllOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--subject")] string Subject,
[property: CliOption("--token")] string Token
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}