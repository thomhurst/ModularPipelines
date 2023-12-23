using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "start-file-transfer")]
public record AwsTransferStartFileTransferOptions(
[property: CommandSwitch("--connector-id")] string ConnectorId
) : AwsOptions
{
    [CommandSwitch("--send-file-paths")]
    public string[]? SendFilePaths { get; set; }

    [CommandSwitch("--retrieve-file-paths")]
    public string[]? RetrieveFilePaths { get; set; }

    [CommandSwitch("--local-directory-path")]
    public string? LocalDirectoryPath { get; set; }

    [CommandSwitch("--remote-directory-path")]
    public string? RemoteDirectoryPath { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}