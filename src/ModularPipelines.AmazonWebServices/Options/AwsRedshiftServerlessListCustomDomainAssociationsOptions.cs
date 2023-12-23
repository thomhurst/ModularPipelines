using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "list-custom-domain-associations")]
public record AwsRedshiftServerlessListCustomDomainAssociationsOptions : AwsOptions
{
    [CommandSwitch("--custom-domain-certificate-arn")]
    public string? CustomDomainCertificateArn { get; set; }

    [CommandSwitch("--custom-domain-name")]
    public string? CustomDomainName { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}