using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "put-model-package-group-policy")]
public record AwsSagemakerPutModelPackageGroupPolicyOptions(
[property: CommandSwitch("--model-package-group-name")] string ModelPackageGroupName,
[property: CommandSwitch("--resource-policy")] string ResourcePolicy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}