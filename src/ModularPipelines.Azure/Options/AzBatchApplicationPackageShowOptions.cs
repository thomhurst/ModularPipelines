using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "application", "package", "show")]
public record AzBatchApplicationPackageShowOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--version-name")] string VersionName
) : AzOptions;