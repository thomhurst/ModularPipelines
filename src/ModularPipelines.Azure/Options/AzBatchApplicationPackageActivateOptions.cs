using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "application", "package", "activate")]
public record AzBatchApplicationPackageActivateOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--format")] string Format,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--version-name")] string VersionName
) : AzOptions;