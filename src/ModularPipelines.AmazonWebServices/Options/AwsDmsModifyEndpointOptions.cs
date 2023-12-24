using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "modify-endpoint")]
public record AwsDmsModifyEndpointOptions(
[property: CommandSwitch("--endpoint-arn")] string EndpointArn
) : AwsOptions
{
    [CommandSwitch("--endpoint-identifier")]
    public string? EndpointIdentifier { get; set; }

    [CommandSwitch("--endpoint-type")]
    public string? EndpointType { get; set; }

    [CommandSwitch("--engine-name")]
    public string? EngineName { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--server-name")]
    public string? ServerName { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--database-name")]
    public string? DatabaseName { get; set; }

    [CommandSwitch("--extra-connection-attributes")]
    public string? ExtraConnectionAttributes { get; set; }

    [CommandSwitch("--certificate-arn")]
    public string? CertificateArn { get; set; }

    [CommandSwitch("--ssl-mode")]
    public string? SslMode { get; set; }

    [CommandSwitch("--service-access-role-arn")]
    public string? ServiceAccessRoleArn { get; set; }

    [CommandSwitch("--external-table-definition")]
    public string? ExternalTableDefinition { get; set; }

    [CommandSwitch("--dynamo-db-settings")]
    public string? DynamoDbSettings { get; set; }

    [CommandSwitch("--s3-settings")]
    public string? S3Settings { get; set; }

    [CommandSwitch("--dms-transfer-settings")]
    public string? DmsTransferSettings { get; set; }

    [CommandSwitch("--mongo-db-settings")]
    public string? MongoDbSettings { get; set; }

    [CommandSwitch("--kinesis-settings")]
    public string? KinesisSettings { get; set; }

    [CommandSwitch("--kafka-settings")]
    public string? KafkaSettings { get; set; }

    [CommandSwitch("--elasticsearch-settings")]
    public string? ElasticsearchSettings { get; set; }

    [CommandSwitch("--neptune-settings")]
    public string? NeptuneSettings { get; set; }

    [CommandSwitch("--redshift-settings")]
    public string? RedshiftSettings { get; set; }

    [CommandSwitch("--postgre-sql-settings")]
    public string? PostgreSqlSettings { get; set; }

    [CommandSwitch("--my-sql-settings")]
    public string? MySqlSettings { get; set; }

    [CommandSwitch("--oracle-settings")]
    public string? OracleSettings { get; set; }

    [CommandSwitch("--sybase-settings")]
    public string? SybaseSettings { get; set; }

    [CommandSwitch("--microsoft-sql-server-settings")]
    public string? MicrosoftSqlServerSettings { get; set; }

    [CommandSwitch("--ibm-db2-settings")]
    public string? IbmDb2Settings { get; set; }

    [CommandSwitch("--doc-db-settings")]
    public string? DocDbSettings { get; set; }

    [CommandSwitch("--redis-settings")]
    public string? RedisSettings { get; set; }

    [CommandSwitch("--gcp-my-sql-settings")]
    public string? GcpMySqlSettings { get; set; }

    [CommandSwitch("--timestream-settings")]
    public string? TimestreamSettings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}