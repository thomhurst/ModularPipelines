using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "operations", "describe")]
public record GcloudStorageOperationsDescribeOptions(
[property: CliArgument] string OperationName
) : GcloudOptions;