using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "describe-rules-packages")]
public record AwsInspectorDescribeRulesPackagesOptions(
[property: CliOption("--rules-package-arns")] string[] RulesPackageArns
) : AwsOptions
{
    [CliOption("--locale")]
    public string? Locale { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}