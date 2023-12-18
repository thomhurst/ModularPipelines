using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "profile", "create")]
public record AzAfdProfileCreateOptions(
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [CommandSwitch("--origin-response-timeout-seconds")]
    public string? OriginResponseTimeoutSeconds { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}