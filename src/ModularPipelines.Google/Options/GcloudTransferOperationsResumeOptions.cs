using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "operations", "resume")]
public record GcloudTransferOperationsResumeOptions(
[property: CliArgument] string Name
) : GcloudOptions;