using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "namespaces", "list")]
public record GcloudServiceDirectoryNamespacesListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;