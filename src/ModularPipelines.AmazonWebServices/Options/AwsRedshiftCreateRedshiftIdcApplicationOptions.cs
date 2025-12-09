using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "create-redshift-idc-application")]
public record AwsRedshiftCreateRedshiftIdcApplicationOptions(
[property: CliOption("--idc-instance-arn")] string IdcInstanceArn,
[property: CliOption("--redshift-idc-application-name")] string RedshiftIdcApplicationName,
[property: CliOption("--idc-display-name")] string IdcDisplayName,
[property: CliOption("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CliOption("--identity-namespace")]
    public string? IdentityNamespace { get; set; }

    [CliOption("--authorized-token-issuer-list")]
    public string[]? AuthorizedTokenIssuerList { get; set; }

    [CliOption("--service-integrations")]
    public string[]? ServiceIntegrations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}