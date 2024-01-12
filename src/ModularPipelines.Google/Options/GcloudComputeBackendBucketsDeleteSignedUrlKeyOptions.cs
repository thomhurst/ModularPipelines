using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "backend-buckets", "delete-signed-url-key")]
public record GcloudComputeBackendBucketsDeleteSignedUrlKeyOptions(
[property: PositionalArgument] string BackendBucketName,
[property: CommandSwitch("--key-name")] string KeyName
) : GcloudOptions;