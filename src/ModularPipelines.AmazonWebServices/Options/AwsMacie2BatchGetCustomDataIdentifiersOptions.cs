using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("macie2", "batch-get-custom-data-identifiers")]
public record AwsMacie2BatchGetCustomDataIdentifiersOptions : AwsOptions
{
    [CommandSwitch("--ids")]
    public string[]? Ids { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}