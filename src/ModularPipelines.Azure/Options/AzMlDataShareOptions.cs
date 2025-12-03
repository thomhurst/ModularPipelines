using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "data", "share")]
public record AzMlDataShareOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry-name")] string RegistryName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--share-with-name")] string ShareWithName,
[property: CliOption("--share-with-version")] string ShareWithVersion,
[property: CliOption("--version")] string Version,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;