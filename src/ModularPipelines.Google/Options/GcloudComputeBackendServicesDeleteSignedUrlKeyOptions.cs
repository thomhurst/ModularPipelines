using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "backend-services", "delete-signed-url-key")]
public record GcloudComputeBackendServicesDeleteSignedUrlKeyOptions(
[property: PositionalArgument] string BackendServiceName,
[property: CommandSwitch("--key-name")] string KeyName
) : GcloudOptions;