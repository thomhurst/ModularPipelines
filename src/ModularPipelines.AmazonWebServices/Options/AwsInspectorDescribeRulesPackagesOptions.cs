using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector", "describe-rules-packages")]
public record AwsInspectorDescribeRulesPackagesOptions(
[property: CommandSwitch("--rules-package-arns")] string[] RulesPackageArns
) : AwsOptions
{
    [CommandSwitch("--locale")]
    public string? Locale { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}