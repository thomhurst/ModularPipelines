using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fsx", "create-data-repository-task")]
public record AwsFsxCreateDataRepositoryTaskOptions(
[property: CliOption("--type")] string Type,
[property: CliOption("--file-system-id")] string FileSystemId,
[property: CliOption("--report")] string Report
) : AwsOptions
{
    [CliOption("--paths")]
    public string[]? Paths { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--capacity-to-release")]
    public long? CapacityToRelease { get; set; }

    [CliOption("--release-configuration")]
    public string? ReleaseConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}