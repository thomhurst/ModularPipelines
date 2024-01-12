using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "operations", "monitor")]
public record GcloudTransferOperationsMonitorOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;