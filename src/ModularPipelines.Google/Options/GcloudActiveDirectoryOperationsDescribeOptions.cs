using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "operations", "describe")]
public record GcloudActiveDirectoryOperationsDescribeOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions;