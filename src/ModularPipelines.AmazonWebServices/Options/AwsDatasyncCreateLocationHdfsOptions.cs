using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "create-location-hdfs")]
public record AwsDatasyncCreateLocationHdfsOptions(
[property: CliOption("--name-nodes")] string[] NameNodes,
[property: CliOption("--authentication-type")] string AuthenticationType,
[property: CliOption("--agent-arns")] string[] AgentArns
) : AwsOptions
{
    [CliOption("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CliOption("--block-size")]
    public int? BlockSize { get; set; }

    [CliOption("--replication-factor")]
    public int? ReplicationFactor { get; set; }

    [CliOption("--kms-key-provider-uri")]
    public string? KmsKeyProviderUri { get; set; }

    [CliOption("--qop-configuration")]
    public string? QopConfiguration { get; set; }

    [CliOption("--simple-user")]
    public string? SimpleUser { get; set; }

    [CliOption("--kerberos-principal")]
    public string? KerberosPrincipal { get; set; }

    [CliOption("--kerberos-keytab")]
    public string? KerberosKeytab { get; set; }

    [CliOption("--kerberos-krb5-conf")]
    public string? KerberosKrb5Conf { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}