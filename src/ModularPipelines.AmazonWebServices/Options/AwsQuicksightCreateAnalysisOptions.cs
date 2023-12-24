using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "create-analysis")]
public record AwsQuicksightCreateAnalysisOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--analysis-id")] string AnalysisId,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--parameters")]
    public string? Parameters { get; set; }

    [CommandSwitch("--permissions")]
    public string[]? Permissions { get; set; }

    [CommandSwitch("--source-entity")]
    public string? SourceEntity { get; set; }

    [CommandSwitch("--theme-arn")]
    public string? ThemeArn { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--definition")]
    public string? Definition { get; set; }

    [CommandSwitch("--validation-strategy")]
    public string? ValidationStrategy { get; set; }

    [CommandSwitch("--folder-arns")]
    public string[]? FolderArns { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}