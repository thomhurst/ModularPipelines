using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("components", "install")]
public record GcloudComponentsInstallOptions(
[property: PositionalArgument] string ComponentIds
) : GcloudOptions;