using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "backend-buckets", "delete")]
public record GcloudComputeBackendBucketsDeleteOptions(
[property: PositionalArgument] string BackendBucketName
) : GcloudOptions;