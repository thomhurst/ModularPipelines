using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "subscribe")]
public record AwsConfigserviceSubscribeOptions(
[property: CliOption("--s3-bucket")] string S3Bucket,
[property: CliOption("--sns-topic")] string SnsTopic,
[property: CliOption("--iam-role")] string IamRole
) : AwsOptions;