using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "services", "delete")]
public record GcloudServiceDirectoryServicesDeleteOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location,
[property: CliArgument] string Namespace
) : GcloudOptions;