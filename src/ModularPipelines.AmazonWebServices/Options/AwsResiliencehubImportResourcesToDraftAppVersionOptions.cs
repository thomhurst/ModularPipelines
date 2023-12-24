using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "import-resources-to-draft-app-version")]
public record AwsResiliencehubImportResourcesToDraftAppVersionOptions(
[property: CommandSwitch("--app-arn")] string AppArn
) : AwsOptions
{
    [CommandSwitch("--eks-sources")]
    public string[]? EksSources { get; set; }

    [CommandSwitch("--import-strategy")]
    public string? ImportStrategy { get; set; }

    [CommandSwitch("--source-arns")]
    public string[]? SourceArns { get; set; }

    [CommandSwitch("--terraform-sources")]
    public string[]? TerraformSources { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}