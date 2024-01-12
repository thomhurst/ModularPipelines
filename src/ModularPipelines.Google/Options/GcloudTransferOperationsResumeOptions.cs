using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "operations", "resume")]
public record GcloudTransferOperationsResumeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;