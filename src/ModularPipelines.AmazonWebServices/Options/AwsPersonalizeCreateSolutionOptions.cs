using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "create-solution")]
public record AwsPersonalizeCreateSolutionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--dataset-group-arn")] string DatasetGroupArn
) : AwsOptions
{
    [CommandSwitch("--recipe-arn")]
    public string? RecipeArn { get; set; }

    [CommandSwitch("--event-type")]
    public string? EventType { get; set; }

    [CommandSwitch("--solution-config")]
    public string? SolutionConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}