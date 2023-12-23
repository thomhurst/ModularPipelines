using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "update-dataset")]
public record AwsPersonalizeUpdateDatasetOptions(
[property: CommandSwitch("--dataset-arn")] string DatasetArn,
[property: CommandSwitch("--schema-arn")] string SchemaArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}