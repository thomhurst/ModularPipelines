using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("verifiedpermissions", "get-schema")]
public record AwsVerifiedpermissionsGetSchemaOptions(
[property: CommandSwitch("--policy-store-id")] string PolicyStoreId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}