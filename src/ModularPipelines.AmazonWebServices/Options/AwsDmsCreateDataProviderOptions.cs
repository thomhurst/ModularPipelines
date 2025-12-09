using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "create-data-provider")]
public record AwsDmsCreateDataProviderOptions(
[property: CliOption("--engine")] string Engine,
[property: CliOption("--settings")] string Settings
) : AwsOptions
{
    [CliOption("--data-provider-name")]
    public string? DataProviderName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}