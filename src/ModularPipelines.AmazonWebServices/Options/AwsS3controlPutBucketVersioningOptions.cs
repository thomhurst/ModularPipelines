using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "put-bucket-versioning")]
public record AwsS3controlPutBucketVersioningOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--versioning-configuration")] string VersioningConfiguration
) : AwsOptions
{
    [CommandSwitch("--mfa")]
    public string? Mfa { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}