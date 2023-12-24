using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("macie2", "update-reveal-configuration")]
public record AwsMacie2UpdateRevealConfigurationOptions(
[property: CommandSwitch("--configuration")] string Configuration
) : AwsOptions
{
    [CommandSwitch("--retrieval-configuration")]
    public string? RetrievalConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}