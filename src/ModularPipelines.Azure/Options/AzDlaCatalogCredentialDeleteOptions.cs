using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dla", "catalog", "credential", "delete")]
public record AzDlaCatalogCredentialDeleteOptions(
[property: CliOption("--credential-name")] string CredentialName,
[property: CliOption("--database-name")] string DatabaseName
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliFlag("--cascade")]
    public bool? Cascade { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}