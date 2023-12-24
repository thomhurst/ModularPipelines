using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "create-publishing-destination")]
public record AwsGuarddutyCreatePublishingDestinationOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--destination-type")] string DestinationType,
[property: CommandSwitch("--destination-properties")] string DestinationProperties
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}