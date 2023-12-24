using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "get-access-grants-instance-for-prefix")]
public record AwsS3controlGetAccessGrantsInstanceForPrefixOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--s3-prefix")] string S3Prefix
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}