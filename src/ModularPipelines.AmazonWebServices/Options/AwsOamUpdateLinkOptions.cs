using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("oam", "update-link")]
public record AwsOamUpdateLinkOptions(
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--resource-types")] string[] ResourceTypes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}