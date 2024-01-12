using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-directory", "namespaces", "describe")]
public record GcloudServiceDirectoryNamespacesDescribeOptions(
[property: PositionalArgument] string Namespace,
[property: PositionalArgument] string Location
) : GcloudOptions;