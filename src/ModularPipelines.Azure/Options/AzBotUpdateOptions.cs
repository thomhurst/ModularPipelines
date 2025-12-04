using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("bot", "update")]
public record AzBotUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--ai-api-key")]
    public string? AiApiKey { get; set; }

    [CliOption("--ai-app-id")]
    public string? AiAppId { get; set; }

    [CliOption("--ai-key")]
    public string? AiKey { get; set; }

    [CliOption("--cmk")]
    public string? Cmk { get; set; }

    [CliOption("--cmk-off")]
    public string? CmkOff { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--icon-url")]
    public string? IconUrl { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}