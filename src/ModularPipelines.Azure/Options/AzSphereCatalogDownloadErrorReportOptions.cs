using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "catalog", "download-error-report")]
public record AzSphereCatalogDownloadErrorReportOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--output-file")]
    public string? OutputFile { get; set; }
}