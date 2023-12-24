using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataexchange", "update-revision")]
public record AwsDataexchangeUpdateRevisionOptions(
[property: CommandSwitch("--data-set-id")] string DataSetId,
[property: CommandSwitch("--revision-id")] string RevisionId
) : AwsOptions
{
    [CommandSwitch("--comment")]
    public string? Comment { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}