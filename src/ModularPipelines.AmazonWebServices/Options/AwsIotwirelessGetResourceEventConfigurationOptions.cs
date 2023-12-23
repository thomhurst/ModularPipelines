using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "get-resource-event-configuration")]
public record AwsIotwirelessGetResourceEventConfigurationOptions(
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--identifier-type")] string IdentifierType
) : AwsOptions
{
    [CommandSwitch("--partner-type")]
    public string? PartnerType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}