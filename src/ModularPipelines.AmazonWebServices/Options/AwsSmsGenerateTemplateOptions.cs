using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sms", "generate-template")]
public record AwsSmsGenerateTemplateOptions : AwsOptions
{
    [CliOption("--app-id")]
    public string? AppId { get; set; }

    [CliOption("--template-format")]
    public string? TemplateFormat { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}