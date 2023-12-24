using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fsx", "associate-file-system-aliases")]
public record AwsFsxAssociateFileSystemAliasesOptions(
[property: CommandSwitch("--file-system-id")] string FileSystemId,
[property: CommandSwitch("--aliases")] string[] Aliases
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}