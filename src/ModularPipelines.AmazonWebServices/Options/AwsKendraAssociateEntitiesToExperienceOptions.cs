using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "associate-entities-to-experience")]
public record AwsKendraAssociateEntitiesToExperienceOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--entity-list")] string[] EntityList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}