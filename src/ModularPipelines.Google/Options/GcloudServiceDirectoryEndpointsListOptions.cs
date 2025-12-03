using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "endpoints", "list")]
public record GcloudServiceDirectoryEndpointsListOptions(
[property: CliOption("--service")] string Service,
[property: CliOption("--location")] string Location,
[property: CliOption("--namespace")] string Namespace
) : GcloudOptions;