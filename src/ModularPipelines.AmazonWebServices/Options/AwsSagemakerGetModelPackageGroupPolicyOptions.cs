using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "get-model-package-group-policy")]
public record AwsSagemakerGetModelPackageGroupPolicyOptions(
[property: CommandSwitch("--model-package-group-name")] string ModelPackageGroupName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}