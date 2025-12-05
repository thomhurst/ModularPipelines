using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcases", "create-template")]
public record AwsConnectcasesCreateTemplateOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--layout-configuration")]
    public string? LayoutConfiguration { get; set; }

    [CliOption("--required-fields")]
    public string[]? RequiredFields { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}