using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-application", "create")]
public record AzSfManagedApplicationCreateOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--application-type-name")] string ApplicationTypeName,
[property: CommandSwitch("--application-type-version")] string ApplicationTypeVersion,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--application-parameters")]
    public string? ApplicationParameters { get; set; }

    [CommandSwitch("--package-url")]
    public string? PackageUrl { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}