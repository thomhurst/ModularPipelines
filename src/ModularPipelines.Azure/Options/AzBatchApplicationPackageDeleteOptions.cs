using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "application", "package", "delete")]
public record AzBatchApplicationPackageDeleteOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--version-name")] string VersionName
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}