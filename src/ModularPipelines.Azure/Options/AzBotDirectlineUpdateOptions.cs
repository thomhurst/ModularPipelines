using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("bot", "directline", "update")]
public record AzBotDirectlineUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--add-disabled")]
    public bool? AddDisabled { get; set; }

    [CliFlag("--disablev1")]
    public bool? Disablev1 { get; set; }

    [CliFlag("--disablev3")]
    public bool? Disablev3 { get; set; }

    [CliFlag("--enable-enhanced-auth")]
    public bool? EnableEnhancedAuth { get; set; }

    [CliOption("--site-name")]
    public string? SiteName { get; set; }

    [CliOption("--trusted-origins")]
    public string? TrustedOrigins { get; set; }
}