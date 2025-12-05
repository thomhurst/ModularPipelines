using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "put-model-package-group-policy")]
public record AwsSagemakerPutModelPackageGroupPolicyOptions(
[property: CliOption("--model-package-group-name")] string ModelPackageGroupName,
[property: CliOption("--resource-policy")] string ResourcePolicy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}