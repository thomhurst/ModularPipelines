using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fsx", "create-data-repository-association")]
public record AwsFsxCreateDataRepositoryAssociationOptions(
[property: CommandSwitch("--file-system-id")] string FileSystemId,
[property: CommandSwitch("--data-repository-path")] string DataRepositoryPath
) : AwsOptions
{
    [CommandSwitch("--file-system-path")]
    public string? FileSystemPath { get; set; }

    [CommandSwitch("--imported-file-chunk-size")]
    public int? ImportedFileChunkSize { get; set; }

    [CommandSwitch("--s3")]
    public string? S3 { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}