using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "model", "package")]
public record AzMlModelPackageOptions(
[property: CliOption("--file")] string File,
[property: CliOption("--name")] string Name,
[property: CliOption("--version")] string Version
) : AzOptions
{
    [CliOption("--registry-name")]
    public string? RegistryName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}