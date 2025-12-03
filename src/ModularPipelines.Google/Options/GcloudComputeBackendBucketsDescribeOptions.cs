using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "backend-buckets", "describe")]
public record GcloudComputeBackendBucketsDescribeOptions(
[property: CliArgument] string BackendBucketName
) : GcloudOptions;