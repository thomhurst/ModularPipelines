using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "promote-resource-share-created-from-policy")]
public record AwsRamPromoteResourceShareCreatedFromPolicyOptions(
[property: CliOption("--resource-share-arn")] string ResourceShareArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}