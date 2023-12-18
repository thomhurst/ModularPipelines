using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-application-type", "version", "update")]
public record AzSfManagedApplicationTypeVersionUpdateOptions(
[property: CommandSwitch("--application-type-name")] string ApplicationTypeName,
[property: CommandSwitch("--application-type-version")] string ApplicationTypeVersion,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--package-url")]
    public string? PackageUrl { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}