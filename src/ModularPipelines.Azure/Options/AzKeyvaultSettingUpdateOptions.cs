using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "setting", "update")]
public record AzKeyvaultSettingUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--value")] string Value
) : AzOptions
{
    [CliOption("--hsm-name")]
    public string? HsmName { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--setting-type")]
    public string? SettingType { get; set; }
}