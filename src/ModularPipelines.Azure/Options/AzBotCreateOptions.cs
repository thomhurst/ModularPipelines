using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bot", "create")]
public record AzBotCreateOptions(
[property: CliOption("--app-type")] string AppType,
[property: CliOption("--appid")] string Appid,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--cmk")]
    public string? Cmk { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--msi-resource-id")]
    public string? MsiResourceId { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }
}