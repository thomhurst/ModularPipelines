using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "delete-model-package-group-policy")]
public record AwsSagemakerDeleteModelPackageGroupPolicyOptions(
[property: CommandSwitch("--model-package-group-name")] string ModelPackageGroupName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}