using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-model-package-group")]
public record AwsSagemakerCreateModelPackageGroupOptions(
[property: CliOption("--model-package-group-name")] string ModelPackageGroupName
) : AwsOptions
{
    [CliOption("--model-package-group-description")]
    public string? ModelPackageGroupDescription { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}