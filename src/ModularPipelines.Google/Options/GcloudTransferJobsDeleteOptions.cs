using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "jobs", "delete")]
public record GcloudTransferJobsDeleteOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;