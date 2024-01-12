using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "operations", "cancel")]
public record GcloudActiveDirectoryOperationsCancelOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;