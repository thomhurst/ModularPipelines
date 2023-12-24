using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codebuild", "create-project")]
public record AwsCodebuildCreateProjectOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--artifacts")] string Artifacts,
[property: CommandSwitch("--environment")] string Environment,
[property: CommandSwitch("--service-role")] string ServiceRole
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--secondary-sources")]
    public string[]? SecondarySources { get; set; }

    [CommandSwitch("--source-version")]
    public string? SourceVersion { get; set; }

    [CommandSwitch("--secondary-source-versions")]
    public string[]? SecondarySourceVersions { get; set; }

    [CommandSwitch("--secondary-artifacts")]
    public string[]? SecondaryArtifacts { get; set; }

    [CommandSwitch("--cache")]
    public string? Cache { get; set; }

    [CommandSwitch("--timeout-in-minutes")]
    public int? TimeoutInMinutes { get; set; }

    [CommandSwitch("--queued-timeout-in-minutes")]
    public int? QueuedTimeoutInMinutes { get; set; }

    [CommandSwitch("--encryption-key")]
    public string? EncryptionKey { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CommandSwitch("--logs-config")]
    public string? LogsConfig { get; set; }

    [CommandSwitch("--file-system-locations")]
    public string[]? FileSystemLocations { get; set; }

    [CommandSwitch("--build-batch-config")]
    public string? BuildBatchConfig { get; set; }

    [CommandSwitch("--concurrent-build-limit")]
    public int? ConcurrentBuildLimit { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}