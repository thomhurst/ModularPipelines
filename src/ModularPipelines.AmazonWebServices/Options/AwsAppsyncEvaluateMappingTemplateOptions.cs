using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "evaluate-mapping-template")]
public record AwsAppsyncEvaluateMappingTemplateOptions(
[property: CliOption("--template")] string Template,
[property: CliOption("--context")] string Context
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}