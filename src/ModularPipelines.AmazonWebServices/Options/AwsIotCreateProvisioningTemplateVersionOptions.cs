using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-provisioning-template-version")]
public record AwsIotCreateProvisioningTemplateVersionOptions(
[property: CliOption("--template-name")] string TemplateName,
[property: CliOption("--template-body")] string TemplateBody
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}