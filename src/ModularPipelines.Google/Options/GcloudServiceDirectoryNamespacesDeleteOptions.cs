using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "namespaces", "delete")]
public record GcloudServiceDirectoryNamespacesDeleteOptions(
[property: CliArgument] string Namespace,
[property: CliArgument] string Location
) : GcloudOptions;