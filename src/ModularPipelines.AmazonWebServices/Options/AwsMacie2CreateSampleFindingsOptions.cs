using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("macie2", "create-sample-findings")]
public record AwsMacie2CreateSampleFindingsOptions : AwsOptions
{
    [CommandSwitch("--finding-types")]
    public string[]? FindingTypes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}