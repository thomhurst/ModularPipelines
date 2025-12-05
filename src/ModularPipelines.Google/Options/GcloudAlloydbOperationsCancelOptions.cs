using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "operations", "cancel")]
public record GcloudAlloydbOperationsCancelOptions(
[property: CliArgument] string Operation,
[property: CliOption("--region")] string Region
) : GcloudOptions;