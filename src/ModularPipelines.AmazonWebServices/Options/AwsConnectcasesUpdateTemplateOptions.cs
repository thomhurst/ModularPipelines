using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcases", "update-template")]
public record AwsConnectcasesUpdateTemplateOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--template-id")] string TemplateId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--layout-configuration")]
    public string? LayoutConfiguration { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--required-fields")]
    public string[]? RequiredFields { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}