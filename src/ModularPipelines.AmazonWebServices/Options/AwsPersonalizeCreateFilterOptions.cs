using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "create-filter")]
public record AwsPersonalizeCreateFilterOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--dataset-group-arn")] string DatasetGroupArn,
[property: CommandSwitch("--filter-expression")] string FilterExpression
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}