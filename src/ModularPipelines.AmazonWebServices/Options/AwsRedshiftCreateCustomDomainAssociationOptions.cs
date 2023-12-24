using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "create-custom-domain-association")]
public record AwsRedshiftCreateCustomDomainAssociationOptions(
[property: CommandSwitch("--custom-domain-name")] string CustomDomainName,
[property: CommandSwitch("--custom-domain-certificate-arn")] string CustomDomainCertificateArn,
[property: CommandSwitch("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}