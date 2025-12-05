using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "backend-buckets", "delete-signed-url-key")]
public record GcloudComputeBackendBucketsDeleteSignedUrlKeyOptions(
[property: CliArgument] string BackendBucketName,
[property: CliOption("--key-name")] string KeyName
) : GcloudOptions;