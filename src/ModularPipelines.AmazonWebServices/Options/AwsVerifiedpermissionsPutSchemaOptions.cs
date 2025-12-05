using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("verifiedpermissions", "put-schema")]
public record AwsVerifiedpermissionsPutSchemaOptions(
[property: CliOption("--policy-store-id")] string PolicyStoreId,
[property: CliOption("--definition")] string Definition
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}