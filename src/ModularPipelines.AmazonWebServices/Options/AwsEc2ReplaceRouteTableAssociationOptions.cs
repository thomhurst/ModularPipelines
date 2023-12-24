using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "replace-route-table-association")]
public record AwsEc2ReplaceRouteTableAssociationOptions(
[property: CommandSwitch("--association-id")] string AssociationId,
[property: CommandSwitch("--route-table-id")] string RouteTableId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}