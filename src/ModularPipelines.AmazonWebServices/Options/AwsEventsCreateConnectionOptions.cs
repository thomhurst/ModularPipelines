using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("events", "create-connection")]
public record AwsEventsCreateConnectionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--authorization-type")] string AuthorizationType,
[property: CommandSwitch("--auth-parameters")] string AuthParameters
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}