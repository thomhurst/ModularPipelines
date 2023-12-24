using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "disassociate-address")]
public record AwsEc2DisassociateAddressOptions : AwsOptions
{
    [CommandSwitch("--association-id")]
    public string? AssociationId { get; set; }

    [CommandSwitch("--public-ip")]
    public string? PublicIp { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}