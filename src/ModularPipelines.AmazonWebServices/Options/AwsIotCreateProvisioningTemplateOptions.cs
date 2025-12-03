using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-provisioning-template")]
public record AwsIotCreateProvisioningTemplateOptions(
[property: CliOption("--template-name")] string TemplateName,
[property: CliOption("--template-body")] string TemplateBody,
[property: CliOption("--provisioning-role-arn")] string ProvisioningRoleArn
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--pre-provisioning-hook")]
    public string? PreProvisioningHook { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}