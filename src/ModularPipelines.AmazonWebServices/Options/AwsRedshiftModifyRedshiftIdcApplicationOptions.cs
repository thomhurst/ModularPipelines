using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "modify-redshift-idc-application")]
public record AwsRedshiftModifyRedshiftIdcApplicationOptions(
[property: CliOption("--redshift-idc-application-arn")] string RedshiftIdcApplicationArn
) : AwsOptions
{
    [CliOption("--identity-namespace")]
    public string? IdentityNamespace { get; set; }

    [CliOption("--iam-role-arn")]
    public string? IamRoleArn { get; set; }

    [CliOption("--idc-display-name")]
    public string? IdcDisplayName { get; set; }

    [CliOption("--authorized-token-issuer-list")]
    public string[]? AuthorizedTokenIssuerList { get; set; }

    [CliOption("--service-integrations")]
    public string[]? ServiceIntegrations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}