using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "update-product")]
public record AwsServicecatalogUpdateProductOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--owner")]
    public string? Owner { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--distributor")]
    public string? Distributor { get; set; }

    [CliOption("--support-description")]
    public string? SupportDescription { get; set; }

    [CliOption("--support-email")]
    public string? SupportEmail { get; set; }

    [CliOption("--support-url")]
    public string? SupportUrl { get; set; }

    [CliOption("--add-tags")]
    public string[]? AddTags { get; set; }

    [CliOption("--remove-tags")]
    public string[]? RemoveTags { get; set; }

    [CliOption("--source-connection")]
    public string? SourceConnection { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}