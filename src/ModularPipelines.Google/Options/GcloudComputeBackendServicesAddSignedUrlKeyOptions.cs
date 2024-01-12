using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "backend-services", "add-signed-url-key")]
public record GcloudComputeBackendServicesAddSignedUrlKeyOptions(
[property: PositionalArgument] string BackendServiceName,
[property: CommandSwitch("--key-file")] string KeyFile,
[property: CommandSwitch("--key-name")] string KeyName
) : GcloudOptions;