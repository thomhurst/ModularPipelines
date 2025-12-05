using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "admin", "banner", "add")]
public record AzDevopsAdminBannerAddOptions(
[property: CliOption("--message")] string Message
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--expiration")]
    public string? Expiration { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}