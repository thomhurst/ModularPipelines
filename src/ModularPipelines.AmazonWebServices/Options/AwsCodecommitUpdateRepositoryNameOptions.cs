using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "update-repository-name")]
public record AwsCodecommitUpdateRepositoryNameOptions(
[property: CommandSwitch("--old-name")] string OldName,
[property: CommandSwitch("--new-name")] string NewName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}