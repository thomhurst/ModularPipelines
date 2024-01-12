using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-directory", "namespaces", "delete")]
public record GcloudServiceDirectoryNamespacesDeleteOptions(
[property: PositionalArgument] string Namespace,
[property: PositionalArgument] string Location
) : GcloudOptions;