using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "update-reveal-configuration")]
public record AwsMacie2UpdateRevealConfigurationOptions(
[property: CliOption("--configuration")] string Configuration
) : AwsOptions
{
    [CliOption("--retrieval-configuration")]
    public string? RetrievalConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}