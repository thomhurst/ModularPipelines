using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "update-environment-template-version")]
public record AwsProtonUpdateEnvironmentTemplateVersionOptions(
[property: CliOption("--major-version")] string MajorVersion,
[property: CliOption("--minor-version")] string MinorVersion,
[property: CliOption("--template-name")] string TemplateName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}