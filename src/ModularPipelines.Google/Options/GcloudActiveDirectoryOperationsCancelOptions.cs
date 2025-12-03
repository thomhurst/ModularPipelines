using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "operations", "cancel")]
public record GcloudActiveDirectoryOperationsCancelOptions(
[property: CliArgument] string Name
) : GcloudOptions;