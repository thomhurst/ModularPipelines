using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databrew", "create-project")]
public record AwsDatabrewCreateProjectOptions(
[property: CommandSwitch("--dataset-name")] string DatasetName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--recipe-name")] string RecipeName,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--sample")]
    public string? Sample { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}