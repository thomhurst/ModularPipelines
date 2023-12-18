using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "cors", "add")]
public record AzStorageCorsAddOptions(
[property: CommandSwitch("--methods")] string Methods,
[property: CommandSwitch("--origins")] string Origins,
[property: CommandSwitch("--services")] string Services
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [BooleanCommandSwitch("--allowed-headers")]
    public bool? AllowedHeaders { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--exposed-headers")]
    public string? ExposedHeaders { get; set; }

    [CommandSwitch("--max-age")]
    public string? MaxAge { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}