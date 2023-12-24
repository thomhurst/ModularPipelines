using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune", "describe-event-categories")]
public record AwsNeptuneDescribeEventCategoriesOptions : AwsOptions
{
    [CommandSwitch("--source-type")]
    public string? SourceType { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}