using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "blob-service-properties", "cors-rule", "add")]
public record AzStorageAccountBlobServicePropertiesCorsRuleAddOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: BooleanCommandSwitch("--allowed-methods")] bool AllowedMethods,
[property: BooleanCommandSwitch("--allowed-origins")] bool AllowedOrigins,
[property: CommandSwitch("--max-age")] string MaxAge
) : AzOptions
{
    [BooleanCommandSwitch("--allowed-headers")]
    public bool? AllowedHeaders { get; set; }

    [CommandSwitch("--exposed-headers")]
    public string? ExposedHeaders { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

