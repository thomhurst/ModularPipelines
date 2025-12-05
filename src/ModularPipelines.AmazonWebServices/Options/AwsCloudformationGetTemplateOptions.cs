using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "get-template")]
public record AwsCloudformationGetTemplateOptions : AwsOptions
{
    [CliOption("--stack-name")]
    public string? StackName { get; set; }

    [CliOption("--change-set-name")]
    public string? ChangeSetName { get; set; }

    [CliOption("--template-stage")]
    public string? TemplateStage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}