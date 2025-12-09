using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "get-template-summary")]
public record AwsCloudformationGetTemplateSummaryOptions : AwsOptions
{
    [CliOption("--template-body")]
    public string? TemplateBody { get; set; }

    [CliOption("--template-url")]
    public string? TemplateUrl { get; set; }

    [CliOption("--stack-name")]
    public string? StackName { get; set; }

    [CliOption("--stack-set-name")]
    public string? StackSetName { get; set; }

    [CliOption("--call-as")]
    public string? CallAs { get; set; }

    [CliOption("--template-summary-config")]
    public string? TemplateSummaryConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}