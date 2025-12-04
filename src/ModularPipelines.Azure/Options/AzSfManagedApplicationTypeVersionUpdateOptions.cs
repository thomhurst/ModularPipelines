using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "managed-application-type", "version", "update")]
public record AzSfManagedApplicationTypeVersionUpdateOptions(
[property: CliOption("--application-type-name")] string ApplicationTypeName,
[property: CliOption("--application-type-version")] string ApplicationTypeVersion,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--package-url")]
    public string? PackageUrl { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}