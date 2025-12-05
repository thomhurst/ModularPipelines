using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "backend-services", "add-signed-url-key")]
public record GcloudComputeBackendServicesAddSignedUrlKeyOptions(
[property: CliArgument] string BackendServiceName,
[property: CliOption("--key-file")] string KeyFile,
[property: CliOption("--key-name")] string KeyName
) : GcloudOptions;