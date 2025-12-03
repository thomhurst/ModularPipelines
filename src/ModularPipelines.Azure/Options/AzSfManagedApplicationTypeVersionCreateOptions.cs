using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sf", "managed-application-type", "version", "create")]
public record AzSfManagedApplicationTypeVersionCreateOptions(
[property: CliOption("--application-type-name")] string ApplicationTypeName,
[property: CliOption("--application-type-version")] string ApplicationTypeVersion,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--package-url")] string PackageUrl,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--tags")]
    public string? Tags { get; set; }
}