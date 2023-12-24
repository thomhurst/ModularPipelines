using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "modify-redshift-idc-application")]
public record AwsRedshiftModifyRedshiftIdcApplicationOptions(
[property: CommandSwitch("--redshift-idc-application-arn")] string RedshiftIdcApplicationArn
) : AwsOptions
{
    [CommandSwitch("--identity-namespace")]
    public string? IdentityNamespace { get; set; }

    [CommandSwitch("--iam-role-arn")]
    public string? IamRoleArn { get; set; }

    [CommandSwitch("--idc-display-name")]
    public string? IdcDisplayName { get; set; }

    [CommandSwitch("--authorized-token-issuer-list")]
    public string[]? AuthorizedTokenIssuerList { get; set; }

    [CommandSwitch("--service-integrations")]
    public string[]? ServiceIntegrations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}