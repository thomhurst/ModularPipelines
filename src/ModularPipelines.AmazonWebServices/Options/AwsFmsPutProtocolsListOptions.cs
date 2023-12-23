using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fms", "put-protocols-list")]
public record AwsFmsPutProtocolsListOptions(
[property: CommandSwitch("--protocols-list")] string ProtocolsList
) : AwsOptions
{
    [CommandSwitch("--tag-list")]
    public string[]? TagList { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}