using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "create-endpoint")]
public record AwsDmsCreateEndpointOptions(
[property: CliOption("--endpoint-identifier")] string EndpointIdentifier,
[property: CliOption("--endpoint-type")] string EndpointType,
[property: CliOption("--engine-name")] string EngineName
) : AwsOptions
{
    [CliOption("--username")]
    public string? Username { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--server-name")]
    public string? ServerName { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliOption("--extra-connection-attributes")]
    public string? ExtraConnectionAttributes { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--certificate-arn")]
    public string? CertificateArn { get; set; }

    [CliOption("--ssl-mode")]
    public string? SslMode { get; set; }

    [CliOption("--service-access-role-arn")]
    public string? ServiceAccessRoleArn { get; set; }

    [CliOption("--external-table-definition")]
    public string? ExternalTableDefinition { get; set; }

    [CliOption("--dynamo-db-settings")]
    public string? DynamoDbSettings { get; set; }

    [CliOption("--s3-settings")]
    public string? S3Settings { get; set; }

    [CliOption("--dms-transfer-settings")]
    public string? DmsTransferSettings { get; set; }

    [CliOption("--mongo-db-settings")]
    public string? MongoDbSettings { get; set; }

    [CliOption("--kinesis-settings")]
    public string? KinesisSettings { get; set; }

    [CliOption("--kafka-settings")]
    public string? KafkaSettings { get; set; }

    [CliOption("--elasticsearch-settings")]
    public string? ElasticsearchSettings { get; set; }

    [CliOption("--neptune-settings")]
    public string? NeptuneSettings { get; set; }

    [CliOption("--redshift-settings")]
    public string? RedshiftSettings { get; set; }

    [CliOption("--postgre-sql-settings")]
    public string? PostgreSqlSettings { get; set; }

    [CliOption("--my-sql-settings")]
    public string? MySqlSettings { get; set; }

    [CliOption("--oracle-settings")]
    public string? OracleSettings { get; set; }

    [CliOption("--sybase-settings")]
    public string? SybaseSettings { get; set; }

    [CliOption("--microsoft-sql-server-settings")]
    public string? MicrosoftSqlServerSettings { get; set; }

    [CliOption("--ibm-db2-settings")]
    public string? IbmDb2Settings { get; set; }

    [CliOption("--resource-identifier")]
    public string? ResourceIdentifier { get; set; }

    [CliOption("--doc-db-settings")]
    public string? DocDbSettings { get; set; }

    [CliOption("--redis-settings")]
    public string? RedisSettings { get; set; }

    [CliOption("--gcp-my-sql-settings")]
    public string? GcpMySqlSettings { get; set; }

    [CliOption("--timestream-settings")]
    public string? TimestreamSettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}