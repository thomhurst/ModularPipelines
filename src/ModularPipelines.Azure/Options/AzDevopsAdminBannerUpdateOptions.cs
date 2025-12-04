using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "admin", "banner", "update")]
public record AzDevopsAdminBannerUpdateOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--expiration")]
    public string? Expiration { get; set; }

    [CliOption("--message")]
    public string? Message { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}