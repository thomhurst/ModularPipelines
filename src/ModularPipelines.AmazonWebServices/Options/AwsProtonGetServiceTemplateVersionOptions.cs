using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "get-service-template-version")]
public record AwsProtonGetServiceTemplateVersionOptions(
[property: CliOption("--major-version")] string MajorVersion,
[property: CliOption("--minor-version")] string MinorVersion,
[property: CliOption("--template-name")] string TemplateName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}