using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "delete-license")]
public record AwsLicenseManagerDeleteLicenseOptions(
[property: CommandSwitch("--license-arn")] string LicenseArn,
[property: CommandSwitch("--source-version")] string SourceVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}