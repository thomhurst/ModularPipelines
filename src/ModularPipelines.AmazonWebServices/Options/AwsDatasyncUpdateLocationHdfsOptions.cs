using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "update-location-hdfs")]
public record AwsDatasyncUpdateLocationHdfsOptions(
[property: CommandSwitch("--location-arn")] string LocationArn
) : AwsOptions
{
    [CommandSwitch("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CommandSwitch("--name-nodes")]
    public string[]? NameNodes { get; set; }

    [CommandSwitch("--block-size")]
    public int? BlockSize { get; set; }

    [CommandSwitch("--replication-factor")]
    public int? ReplicationFactor { get; set; }

    [CommandSwitch("--kms-key-provider-uri")]
    public string? KmsKeyProviderUri { get; set; }

    [CommandSwitch("--qop-configuration")]
    public string? QopConfiguration { get; set; }

    [CommandSwitch("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CommandSwitch("--simple-user")]
    public string? SimpleUser { get; set; }

    [CommandSwitch("--kerberos-principal")]
    public string? KerberosPrincipal { get; set; }

    [CommandSwitch("--kerberos-keytab")]
    public string? KerberosKeytab { get; set; }

    [CommandSwitch("--kerberos-krb5-conf")]
    public string? KerberosKrb5Conf { get; set; }

    [CommandSwitch("--agent-arns")]
    public string[]? AgentArns { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}