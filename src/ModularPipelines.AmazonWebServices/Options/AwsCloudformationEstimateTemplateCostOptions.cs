using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "estimate-template-cost")]
public record AwsCloudformationEstimateTemplateCostOptions : AwsOptions
{
    [CliOption("--template-body")]
    public string? TemplateBody { get; set; }

    [CliOption("--template-url")]
    public string? TemplateUrl { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}