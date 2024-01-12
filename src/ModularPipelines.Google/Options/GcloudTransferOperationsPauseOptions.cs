using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "operations", "pause")]
public record GcloudTransferOperationsPauseOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;