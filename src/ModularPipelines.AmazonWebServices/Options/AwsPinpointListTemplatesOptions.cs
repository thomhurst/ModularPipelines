using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "list-templates")]
public record AwsPinpointListTemplatesOptions : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--prefix")]
    public string? Prefix { get; set; }

    [CliOption("--template-type")]
    public string? TemplateType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}