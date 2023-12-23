using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-model-package-group")]
public record AwsSagemakerCreateModelPackageGroupOptions(
[property: CommandSwitch("--model-package-group-name")] string ModelPackageGroupName
) : AwsOptions
{
    [CommandSwitch("--model-package-group-description")]
    public string? ModelPackageGroupDescription { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}