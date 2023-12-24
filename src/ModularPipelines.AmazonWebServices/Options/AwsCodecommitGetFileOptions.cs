using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "get-file")]
public record AwsCodecommitGetFileOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--file-path")] string FilePath
) : AwsOptions
{
    [CommandSwitch("--commit-specifier")]
    public string? CommitSpecifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}