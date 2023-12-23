using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pi", "get-dimension-key-details")]
public record AwsPiGetDimensionKeyDetailsOptions(
[property: CommandSwitch("--service-type")] string ServiceType,
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--group")] string Group,
[property: CommandSwitch("--group-identifier")] string GroupIdentifier
) : AwsOptions
{
    [CommandSwitch("--requested-dimensions")]
    public string[]? RequestedDimensions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}