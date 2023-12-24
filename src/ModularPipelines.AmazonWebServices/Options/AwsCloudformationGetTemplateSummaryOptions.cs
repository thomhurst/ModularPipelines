using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "get-template-summary")]
public record AwsCloudformationGetTemplateSummaryOptions : AwsOptions
{
    [CommandSwitch("--template-body")]
    public string? TemplateBody { get; set; }

    [CommandSwitch("--template-url")]
    public string? TemplateUrl { get; set; }

    [CommandSwitch("--stack-name")]
    public string? StackName { get; set; }

    [CommandSwitch("--stack-set-name")]
    public string? StackSetName { get; set; }

    [CommandSwitch("--call-as")]
    public string? CallAs { get; set; }

    [CommandSwitch("--template-summary-config")]
    public string? TemplateSummaryConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}