using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("verifiedpermissions", "get-policy-template")]
public record AwsVerifiedpermissionsGetPolicyTemplateOptions(
[property: CliOption("--policy-store-id")] string PolicyStoreId,
[property: CliOption("--policy-template-id")] string PolicyTemplateId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}