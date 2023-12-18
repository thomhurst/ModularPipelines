using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "admin", "banner", "update")]
public record AzDevopsAdminBannerUpdateOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--expiration")]
    public string? Expiration { get; set; }

    [CommandSwitch("--message")]
    public string? Message { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}