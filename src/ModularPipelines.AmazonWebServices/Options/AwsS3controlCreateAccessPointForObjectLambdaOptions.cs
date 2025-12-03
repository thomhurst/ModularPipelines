using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "create-access-point-for-object-lambda")]
public record AwsS3controlCreateAccessPointForObjectLambdaOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--name")] string Name,
[property: CliOption("--configuration")] string Configuration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}