using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "backend-services", "delete-signed-url-key")]
public record GcloudComputeBackendServicesDeleteSignedUrlKeyOptions(
[property: CliArgument] string BackendServiceName,
[property: CliOption("--key-name")] string KeyName
) : GcloudOptions;