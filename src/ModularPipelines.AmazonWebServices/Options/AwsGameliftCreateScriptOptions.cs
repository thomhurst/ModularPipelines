using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "create-script")]
public record AwsGameliftCreateScriptOptions : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--storage-location")]
    public string? StorageLocation { get; set; }

    [CliOption("--zip-file")]
    public string? ZipFile { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--script-version")]
    public string? ScriptVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}