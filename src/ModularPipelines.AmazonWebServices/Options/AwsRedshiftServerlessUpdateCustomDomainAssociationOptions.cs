using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "update-custom-domain-association")]
public record AwsRedshiftServerlessUpdateCustomDomainAssociationOptions(
[property: CommandSwitch("--custom-domain-certificate-arn")] string CustomDomainCertificateArn,
[property: CommandSwitch("--custom-domain-name")] string CustomDomainName,
[property: CommandSwitch("--workgroup-name")] string WorkgroupName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}