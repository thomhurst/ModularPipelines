using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "update-product")]
public record AwsServicecatalogUpdateProductOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--owner")]
    public string? Owner { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--distributor")]
    public string? Distributor { get; set; }

    [CommandSwitch("--support-description")]
    public string? SupportDescription { get; set; }

    [CommandSwitch("--support-email")]
    public string? SupportEmail { get; set; }

    [CommandSwitch("--support-url")]
    public string? SupportUrl { get; set; }

    [CommandSwitch("--add-tags")]
    public string[]? AddTags { get; set; }

    [CommandSwitch("--remove-tags")]
    public string[]? RemoveTags { get; set; }

    [CommandSwitch("--source-connection")]
    public string? SourceConnection { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}