using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codebuild", "update-project")]
public record AwsCodebuildUpdateProjectOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--secondary-sources")]
    public string[]? SecondarySources { get; set; }

    [CliOption("--source-version")]
    public string? SourceVersion { get; set; }

    [CliOption("--secondary-source-versions")]
    public string[]? SecondarySourceVersions { get; set; }

    [CliOption("--artifacts")]
    public string? Artifacts { get; set; }

    [CliOption("--secondary-artifacts")]
    public string[]? SecondaryArtifacts { get; set; }

    [CliOption("--cache")]
    public string? Cache { get; set; }

    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--service-role")]
    public string? ServiceRole { get; set; }

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