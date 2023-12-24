using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "create-insight")]
public record AwsSecurityhubCreateInsightOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--filters")] string Filters,
[property: CommandSwitch("--group-by-attribute")] string GroupByAttribute
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}