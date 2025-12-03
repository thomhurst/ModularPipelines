using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "delete-provisioning-template-version")]
public record AwsIotDeleteProvisioningTemplateVersionOptions(
[property: CliOption("--template-name")] string TemplateName,
[property: CliOption("--version-id")] int VersionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}