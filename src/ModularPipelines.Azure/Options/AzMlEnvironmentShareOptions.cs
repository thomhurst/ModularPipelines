using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "environment", "share")]
public record AzMlEnvironmentShareOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry-name")] string RegistryName,
[property: CliOption("--share-with-name")] string ShareWithName,
[property: CliOption("--share-with-version")] string ShareWithVersion,
[property: CliOption("--version")] string Version
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}