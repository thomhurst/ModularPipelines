using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bot", "authsetting", "create")]
public record AzBotAuthsettingCreateOptions(
[property: CliOption("--client-id")] string ClientId,
[property: CliOption("--client-secret")] string ClientSecret,
[property: CliOption("--name")] string Name,
[property: CliOption("--provider-scope-string")] string ProviderScopeString,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service,
[property: CliOption("--setting-name")] string SettingName
) : AzOptions
{
    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }
}