using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "create-location-hdfs")]
public record AwsDatasyncCreateLocationHdfsOptions(
[property: CommandSwitch("--name-nodes")] string[] NameNodes,
[property: CommandSwitch("--authentication-type")] string AuthenticationType,
[property: CommandSwitch("--agent-arns")] string[] AgentArns
) : AwsOptions
{
    [CommandSwitch("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CommandSwitch("--block-size")]
    public int? BlockSize { get; set; }

    [CommandSwitch("--replication-factor")]
    public int? ReplicationFactor { get; set; }

    [CommandSwitch("--kms-key-provider-uri")]
    public string? KmsKeyProviderUri { get; set; }

    [CommandSwitch("--qop-configuration")]
    public string? QopConfiguration { get; set; }

    [CommandSwitch("--simple-user")]
    public string? SimpleUser { get; set; }

    [CommandSwitch("--kerberos-principal")]
    public string? KerberosPrincipal { get; set; }

    [CommandSwitch("--kerberos-keytab")]
    public string? KerberosKeytab { get; set; }

    [CommandSwitch("--kerberos-krb5-conf")]
    public string? KerberosKrb5Conf { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}