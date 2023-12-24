using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "subscribe")]
public record AwsConfigserviceSubscribeOptions(
[property: CommandSwitch("--s3-bucket")] string S3Bucket,
[property: CommandSwitch("--sns-topic")] string SnsTopic,
[property: CommandSwitch("--iam-role")] string IamRole
) : AwsOptions;