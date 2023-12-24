using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "create-redshift-idc-application")]
public record AwsRedshiftCreateRedshiftIdcApplicationOptions(
[property: CommandSwitch("--idc-instance-arn")] string IdcInstanceArn,
[property: CommandSwitch("--redshift-idc-application-name")] string RedshiftIdcApplicationName,
[property: CommandSwitch("--idc-display-name")] string IdcDisplayName,
[property: CommandSwitch("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CommandSwitch("--identity-namespace")]
    public string? IdentityNamespace { get; set; }

    [CommandSwitch("--authorized-token-issuer-list")]
    public string[]? AuthorizedTokenIssuerList { get; set; }

    [CommandSwitch("--service-integrations")]
    public string[]? ServiceIntegrations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}