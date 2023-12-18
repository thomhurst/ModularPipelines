using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bot", "directline", "update")]
public record AzBotDirectlineUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--add-disabled")]
    public bool? AddDisabled { get; set; }

    [BooleanCommandSwitch("--disablev1")]
    public bool? Disablev1 { get; set; }

    [BooleanCommandSwitch("--disablev3")]
    public bool? Disablev3 { get; set; }

    [BooleanCommandSwitch("--enable-enhanced-auth")]
    public bool? EnableEnhancedAuth { get; set; }

    [CommandSwitch("--site-name")]
    public string? SiteName { get; set; }

    [CommandSwitch("--trusted-origins")]
    public string? TrustedOrigins { get; set; }
}

