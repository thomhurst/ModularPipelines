using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "detach-thing-principal")]
public record AwsIotDetachThingPrincipalOptions(
[property: CommandSwitch("--thing-name")] string ThingName,
[property: CommandSwitch("--principal")] string Principal
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}