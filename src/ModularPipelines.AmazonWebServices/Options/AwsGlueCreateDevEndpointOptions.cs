using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "create-dev-endpoint")]
public record AwsGlueCreateDevEndpointOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--subnet-id")]
    public string? SubnetId { get; set; }

    [CommandSwitch("--public-key")]
    public string? PublicKey { get; set; }

    [CommandSwitch("--public-keys")]
    public string[]? PublicKeys { get; set; }

    [CommandSwitch("--number-of-nodes")]
    public int? NumberOfNodes { get; set; }

    [CommandSwitch("--worker-type")]
    public string? WorkerType { get; set; }

    [CommandSwitch("--glue-version")]
    public string? GlueVersion { get; set; }

    [CommandSwitch("--number-of-workers")]
    public int? NumberOfWorkers { get; set; }

    [CommandSwitch("--extra-python-libs-s3-path")]
    public string? ExtraPythonLibsS3Path { get; set; }

    [CommandSwitch("--extra-jars-s3-path")]
    public string? ExtraJarsS3Path { get; set; }

    [CommandSwitch("--security-configuration")]
    public string? SecurityConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--arguments")]
    public IEnumerable<KeyValue>? AwsGlueArguments { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}