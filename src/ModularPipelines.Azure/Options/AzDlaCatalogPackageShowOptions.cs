using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dla", "catalog", "package", "show")]
public record AzDlaCatalogPackageShowOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--package-name")] string PackageName,
[property: CliOption("--schema-name")] string SchemaName
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}