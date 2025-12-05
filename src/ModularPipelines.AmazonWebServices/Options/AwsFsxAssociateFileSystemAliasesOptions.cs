using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fsx", "associate-file-system-aliases")]
public record AwsFsxAssociateFileSystemAliasesOptions(
[property: CliOption("--file-system-id")] string FileSystemId,
[property: CliOption("--aliases")] string[] Aliases
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}