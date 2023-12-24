using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("macie2", "get-sensitive-data-occurrences-availability")]
public record AwsMacie2GetSensitiveDataOccurrencesAvailabilityOptions(
[property: CommandSwitch("--finding-id")] string FindingId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}