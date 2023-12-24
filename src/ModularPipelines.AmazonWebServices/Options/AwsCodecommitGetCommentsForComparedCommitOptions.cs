using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "get-comments-for-compared-commit")]
public record AwsCodecommitGetCommentsForComparedCommitOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--after-commit-id")] string AfterCommitId
) : AwsOptions
{
    [CommandSwitch("--before-commit-id")]
    public string? BeforeCommitId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}