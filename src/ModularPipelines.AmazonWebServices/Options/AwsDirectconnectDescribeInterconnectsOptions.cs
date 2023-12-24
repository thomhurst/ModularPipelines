using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "describe-interconnects")]
public record AwsDirectconnectDescribeInterconnectsOptions : AwsOptions
{
    [CommandSwitch("--interconnect-id")]
    public string? InterconnectId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}