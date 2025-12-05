using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "account", "blob-service-properties", "cors-rule", "add")]
public record AzStorageAccountBlobServicePropertiesCorsRuleAddOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliFlag("--allowed-methods")] bool AllowedMethods,
[property: CliFlag("--allowed-origins")] bool AllowedOrigins,
[property: CliOption("--max-age")] string MaxAge
) : AzOptions
{
    [CliFlag("--allowed-headers")]
    public bool? AllowedHeaders { get; set; }

    [CliOption("--exposed-headers")]
    public string? ExposedHeaders { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}