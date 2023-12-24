using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "create-dataset-group")]
public record AwsForecastCreateDatasetGroupOptions(
[property: CommandSwitch("--dataset-group-name")] string DatasetGroupName,
[property: CommandSwitch("--domain")] string Domain
) : AwsOptions
{
    [CommandSwitch("--dataset-arns")]
    public string[]? DatasetArns { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}