using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "get-folder")]
public record AwsCodecommitGetFolderOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--folder-path")] string FolderPath
) : AwsOptions
{
    [CommandSwitch("--commit-specifier")]
    public string? CommitSpecifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}