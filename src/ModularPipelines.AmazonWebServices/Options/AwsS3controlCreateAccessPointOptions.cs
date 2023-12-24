using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "create-access-point")]
public record AwsS3controlCreateAccessPointOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--bucket")] string Bucket
) : AwsOptions
{
    [CommandSwitch("--vpc-configuration")]
    public string? VpcConfiguration { get; set; }

    [CommandSwitch("--public-access-block-configuration")]
    public string? PublicAccessBlockConfiguration { get; set; }

    [CommandSwitch("--bucket-account-id")]
    public string? BucketAccountId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}