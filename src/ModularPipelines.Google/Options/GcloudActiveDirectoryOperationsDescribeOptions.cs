using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "operations", "describe")]
public record GcloudActiveDirectoryOperationsDescribeOptions(
[property: CliArgument] string Operation
) : GcloudOptions;