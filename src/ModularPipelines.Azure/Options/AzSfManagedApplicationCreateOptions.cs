using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "managed-application", "create")]
public record AzSfManagedApplicationCreateOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--application-type-name")] string ApplicationTypeName,
[property: CliOption("--application-type-version")] string ApplicationTypeVersion,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--application-parameters")]
    public string? ApplicationParameters { get; set; }

    [CliOption("--package-url")]
    public string? PackageUrl { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}