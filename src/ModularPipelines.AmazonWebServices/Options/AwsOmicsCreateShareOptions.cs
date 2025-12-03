using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "create-share")]
public record AwsOmicsCreateShareOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--principal-subscriber")] string PrincipalSubscriber
) : AwsOptions
{
    [CliOption("--share-name")]
    public string? ShareName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}