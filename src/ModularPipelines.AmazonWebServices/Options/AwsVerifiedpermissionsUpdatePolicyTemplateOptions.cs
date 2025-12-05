using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("verifiedpermissions", "update-policy-template")]
public record AwsVerifiedpermissionsUpdatePolicyTemplateOptions(
[property: CliOption("--policy-store-id")] string PolicyStoreId,
[property: CliOption("--policy-template-id")] string PolicyTemplateId,
[property: CliOption("--statement")] string Statement
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}