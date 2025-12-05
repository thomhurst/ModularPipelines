using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "backend-buckets", "add-signed-url-key")]
public record GcloudComputeBackendBucketsAddSignedUrlKeyOptions(
[property: CliArgument] string BackendBucketName,
[property: CliOption("--key-file")] string KeyFile,
[property: CliOption("--key-name")] string KeyName
) : GcloudOptions;