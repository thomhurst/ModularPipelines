using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("verifiedpermissions", "put-schema")]
public record AwsVerifiedpermissionsPutSchemaOptions(
[property: CommandSwitch("--policy-store-id")] string PolicyStoreId,
[property: CommandSwitch("--definition")] string Definition
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}