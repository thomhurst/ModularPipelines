using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "start-file-transfer")]
public record AwsTransferStartFileTransferOptions(
[property: CliOption("--connector-id")] string ConnectorId
) : AwsOptions
{
    [CliOption("--send-file-paths")]
    public string[]? SendFilePaths { get; set; }

    [CliOption("--retrieve-file-paths")]
    public string[]? RetrieveFilePaths { get; set; }

    [CliOption("--local-directory-path")]
    public string? LocalDirectoryPath { get; set; }

    [CliOption("--remote-directory-path")]
    public string? RemoteDirectoryPath { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}