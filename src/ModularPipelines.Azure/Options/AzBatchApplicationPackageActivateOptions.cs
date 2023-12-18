using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "application", "package", "activate")]
public record AzBatchApplicationPackageActivateOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--format")] string Format,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--version-name")] string VersionName
) : AzOptions;