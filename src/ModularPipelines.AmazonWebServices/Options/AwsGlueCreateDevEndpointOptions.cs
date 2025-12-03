using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "create-dev-endpoint")]
public record AwsGlueCreateDevEndpointOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--subnet-id")]
    public string? SubnetId { get; set; }

    [CliOption("--public-key")]
    public string? PublicKey { get; set; }

    [CliOption("--public-keys")]
    public string[]? PublicKeys { get; set; }

    [CliOption("--number-of-nodes")]
    public int? NumberOfNodes { get; set; }

    [CliOption("--worker-type")]
    public string? WorkerType { get; set; }

    [CliOption("--glue-version")]
    public string? GlueVersion { get; set; }

    [CliOption("--number-of-workers")]
    public int? NumberOfWorkers { get; set; }

    [CliOption("--extra-python-libs-s3-path")]
    public string? ExtraPythonLibsS3Path { get; set; }

    [CliOption("--extra-jars-s3-path")]
    public string? ExtraJarsS3Path { get; set; }

    [CliOption("--security-configuration")]
    public string? SecurityConfiguration { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--arguments")]
    public IEnumerable<KeyValue>? AwsGlueArguments { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}