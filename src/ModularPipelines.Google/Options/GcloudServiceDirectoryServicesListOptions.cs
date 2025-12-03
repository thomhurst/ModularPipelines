using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "services", "list")]
public record GcloudServiceDirectoryServicesListOptions(
[property: CliOption("--namespace")] string Namespace,
[property: CliOption("--location")] string Location
) : GcloudOptions;