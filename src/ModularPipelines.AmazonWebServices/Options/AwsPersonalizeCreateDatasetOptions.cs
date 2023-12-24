using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "create-dataset")]
public record AwsPersonalizeCreateDatasetOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--schema-arn")] string SchemaArn,
[property: CommandSwitch("--dataset-group-arn")] string DatasetGroupArn,
[property: CommandSwitch("--dataset-type")] string DatasetType
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}