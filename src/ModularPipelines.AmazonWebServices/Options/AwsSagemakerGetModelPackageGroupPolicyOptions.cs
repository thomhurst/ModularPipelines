using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "get-model-package-group-policy")]
public record AwsSagemakerGetModelPackageGroupPolicyOptions(
[property: CliOption("--model-package-group-name")] string ModelPackageGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}