using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-provisioning-template")]
public record AwsIotUpdateProvisioningTemplateOptions(
[property: CliOption("--template-name")] string TemplateName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--default-version-id")]
    public int? DefaultVersionId { get; set; }

    [CliOption("--provisioning-role-arn")]
    public string? ProvisioningRoleArn { get; set; }

    [CliOption("--pre-provisioning-hook")]
    public string? PreProvisioningHook { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}