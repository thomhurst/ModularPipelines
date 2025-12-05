using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "wait", "environment-template-version-registered")]
public record AwsProtonWaitEnvironmentTemplateVersionRegisteredOptions(
[property: CliOption("--major-version")] string MajorVersion,
[property: CliOption("--minor-version")] string MinorVersion,
[property: CliOption("--template-name")] string TemplateName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}