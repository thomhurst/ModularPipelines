using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("verifiedpermissions", "get-schema")]
public record AwsVerifiedpermissionsGetSchemaOptions(
[property: CliOption("--policy-store-id")] string PolicyStoreId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}