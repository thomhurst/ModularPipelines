using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "application", "package", "show")]
public record AzBatchApplicationPackageShowOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--version-name")] string VersionName
) : AzOptions
{
}

