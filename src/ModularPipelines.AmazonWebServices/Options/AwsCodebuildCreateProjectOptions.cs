using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codebuild", "create-project")]
public record AwsCodebuildCreateProjectOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--source")] string Source,
[property: CliOption("--artifacts")] string Artifacts,
[property: CliOption("--environment")] string Environment,
[property: CliOption("--service-role")] string ServiceRole
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--secondary-sources")]
    public string[]? SecondarySources { get; set; }

    [CliOption("--source-version")]
    public string? SourceVersion { get; set; }

    [CliOption("--secondary-source-versions")]
    public string[]? SecondarySourceVersions { get; set; }

    [CliOption("--secondary-artifacts")]
    public string[]? SecondaryArtifacts { get; set; }

    [CliOption("--cache")]
    public string? Cache { get; set; }

    [CliOption("--timeout-in-minutes")]
    public int? TimeoutInMinutes { get; set; }

    [CliOption("--queued-timeout-in-minutes")]
    public int? QueuedTimeoutInMinutes { get; set; }

    [CliOption("--encryption-key")]
    public string? EncryptionKey { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--logs-config")]
    public string? LogsConfig { get; set; }

    [CliOption("--file-system-locations")]
    public string[]? FileSystemLocations { get; set; }

    [CliOption("--build-batch-config")]
    public string? BuildBatchConfig { get; set; }

    [CliOption("--concurrent-build-limit")]
    public int? ConcurrentBuildLimit { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}