using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bot", "authsetting", "list-providers")]
public record AzBotAuthsettingListProvidersOptions : AzOptions
{
    [CommandSwitch("--provider-name")]
    public string? ProviderName { get; set; }
}