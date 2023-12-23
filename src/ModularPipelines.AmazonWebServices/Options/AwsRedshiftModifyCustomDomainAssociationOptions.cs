using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "modify-custom-domain-association")]
public record AwsRedshiftModifyCustomDomainAssociationOptions(
[property: CommandSwitch("--custom-domain-name")] string CustomDomainName,
[property: CommandSwitch("--custom-domain-certificate-arn")] string CustomDomainCertificateArn,
[property: CommandSwitch("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}