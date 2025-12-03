using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "backend-buckets", "delete")]
public record GcloudComputeBackendBucketsDeleteOptions(
[property: CliArgument] string BackendBucketName
) : GcloudOptions;