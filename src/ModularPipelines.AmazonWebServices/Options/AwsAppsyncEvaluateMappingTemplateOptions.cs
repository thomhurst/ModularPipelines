using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "evaluate-mapping-template")]
public record AwsAppsyncEvaluateMappingTemplateOptions(
[property: CommandSwitch("--template")] string Template,
[property: CommandSwitch("--context")] string Context
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}