using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "delete-ml-endpoint")]
public record AwsNeptunedataDeleteMlEndpointOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--neptune-iam-role-arn")]
    public string? NeptuneIamRoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}