using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "get-differences")]
public record AwsCodecommitGetDifferencesOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--after-commit-specifier")] string AfterCommitSpecifier
) : AwsOptions
{
    [CommandSwitch("--before-commit-specifier")]
    public string? BeforeCommitSpecifier { get; set; }

    [CommandSwitch("--before-path")]
    public string? BeforePath { get; set; }

    [CommandSwitch("--after-path")]
    public string? AfterPath { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}