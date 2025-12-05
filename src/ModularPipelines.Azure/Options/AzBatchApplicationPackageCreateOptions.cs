using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("batch", "application", "package", "create")]
public record AzBatchApplicationPackageCreateOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--name")] string Name,
[property: CliOption("--package-file")] string PackageFile,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--version-name")] string VersionName
) : AzOptions;