using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "modify-data-provider")]
public record AwsDmsModifyDataProviderOptions(
[property: CliOption("--data-provider-identifier")] string DataProviderIdentifier
) : AwsOptions
{
    [CliOption("--data-provider-name")]
    public string? DataProviderName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--engine")]
    public string? Engine { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}