using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "get")]
public record GcloudConfigGetOptions(
[property: PositionalArgument] string Section
) : GcloudOptions;