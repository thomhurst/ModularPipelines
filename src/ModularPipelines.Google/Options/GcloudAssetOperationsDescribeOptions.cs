using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("asset", "operations", "describe")]
public record GcloudAssetOperationsDescribeOptions(
[property: CliArgument] string OperationName
) : GcloudOptions;