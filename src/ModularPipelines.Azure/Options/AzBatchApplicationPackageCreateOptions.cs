using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "application", "package", "create")]
public record AzBatchApplicationPackageCreateOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--package-file")] string PackageFile,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--version-name")] string VersionName
) : AzOptions
{
    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

