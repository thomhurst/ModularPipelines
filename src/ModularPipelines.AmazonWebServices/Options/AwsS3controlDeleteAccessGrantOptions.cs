using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "delete-access-grant")]
public record AwsS3controlDeleteAccessGrantOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--access-grant-id")] string AccessGrantId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}