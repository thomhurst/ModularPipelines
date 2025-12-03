using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "video", "operations", "wait")]
public record GcloudMlVideoOperationsWaitOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location
) : GcloudOptions;