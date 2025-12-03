using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devops", "security", "permission", "update")]
public record AzDevopsSecurityPermissionUpdateOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--subject")] string Subject,
[property: CliOption("--token")] string Token
) : AzOptions
{
    [CliFlag("--allow-bit")]
    public bool? AllowBit { get; set; }

    [CliOption("--deny-bit")]
    public string? DenyBit { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliFlag("--merge")]
    public bool? Merge { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }
}