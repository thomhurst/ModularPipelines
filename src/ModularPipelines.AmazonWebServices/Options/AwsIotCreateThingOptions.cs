using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-thing")]
public record AwsIotCreateThingOptions(
[property: CommandSwitch("--thing-name")] string ThingName
) : AwsOptions
{
    [CommandSwitch("--thing-type-name")]
    public string? ThingTypeName { get; set; }

    [CommandSwitch("--attribute-payload")]
    public string? AttributePayload { get; set; }

    [CommandSwitch("--billing-group-name")]
    public string? BillingGroupName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}