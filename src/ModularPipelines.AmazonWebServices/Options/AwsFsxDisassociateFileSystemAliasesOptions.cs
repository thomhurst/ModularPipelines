using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fsx", "disassociate-file-system-aliases")]
public record AwsFsxDisassociateFileSystemAliasesOptions(
[property: CommandSwitch("--file-system-id")] string FileSystemId,
[property: CommandSwitch("--aliases")] string[] Aliases
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}