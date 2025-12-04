using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("bot", "authsetting", "list-providers")]
public record AzBotAuthsettingListProvidersOptions : AzOptions
{
    [CliOption("--provider-name")]
    public string? ProviderName { get; set; }
}