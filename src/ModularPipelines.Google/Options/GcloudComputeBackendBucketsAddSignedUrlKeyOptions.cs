using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "backend-buckets", "add-signed-url-key")]
public record GcloudComputeBackendBucketsAddSignedUrlKeyOptions(
[property: PositionalArgument] string BackendBucketName,
[property: CommandSwitch("--key-file")] string KeyFile,
[property: CommandSwitch("--key-name")] string KeyName
) : GcloudOptions;