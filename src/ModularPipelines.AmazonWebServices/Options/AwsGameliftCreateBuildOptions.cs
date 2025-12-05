using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "create-build")]
public record AwsGameliftCreateBuildOptions : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--storage-location")]
    public string? StorageLocation { get; set; }

    [CliOption("--operating-system")]
    public string? OperatingSystem { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--server-sdk-version")]
    public string? ServerSdkVersion { get; set; }

    [CliOption("--build-version")]
    public string? BuildVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}