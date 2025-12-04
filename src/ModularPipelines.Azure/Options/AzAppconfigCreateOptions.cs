using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appconfig", "create")]
public record AzAppconfigCreateOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CliFlag("--disable-local-auth")]
    public bool? DisableLocalAuth { get; set; }

    [CliFlag("--enable-public-network")]
    public bool? EnablePublicNetwork { get; set; }

    [CliFlag("--enable-purge-protection")]
    public bool? EnablePurgeProtection { get; set; }

    [CliOption("--retention-days")]
    public string? RetentionDays { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}