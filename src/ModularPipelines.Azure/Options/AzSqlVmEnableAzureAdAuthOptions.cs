using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "vm", "enable-ure-ad-auth")]
public record AzSqlVmEnableAzureAdAuthOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--msi-client-id")]
    public string? MsiClientId { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--skip-client-validation")]
    public string? SkipClientValidation { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}