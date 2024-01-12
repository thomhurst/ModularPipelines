using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("services", "operations", "wait")]
public record GcloudServicesOperationsWaitOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions;