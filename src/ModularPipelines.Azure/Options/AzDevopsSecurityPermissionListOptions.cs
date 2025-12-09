using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "security", "permission", "list")]
public record AzDevopsSecurityPermissionListOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--subject")] string Subject
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliFlag("--recurse")]
    public bool? Recurse { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}