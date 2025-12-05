using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "replace-route-table-association")]
public record AwsEc2ReplaceRouteTableAssociationOptions(
[property: CliOption("--association-id")] string AssociationId,
[property: CliOption("--route-table-id")] string RouteTableId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}