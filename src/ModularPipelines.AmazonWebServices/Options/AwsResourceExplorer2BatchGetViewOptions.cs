using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-explorer-2", "batch-get-view")]
public record AwsResourceExplorer2BatchGetViewOptions : AwsOptions
{
    [CommandSwitch("--view-arns")]
    public string[]? ViewArns { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}