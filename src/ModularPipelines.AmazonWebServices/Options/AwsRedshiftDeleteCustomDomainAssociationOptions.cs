using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "delete-custom-domain-association")]
public record AwsRedshiftDeleteCustomDomainAssociationOptions(
[property: CommandSwitch("--cluster-identifier")] string ClusterIdentifier,
[property: CommandSwitch("--custom-domain-name")] string CustomDomainName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}