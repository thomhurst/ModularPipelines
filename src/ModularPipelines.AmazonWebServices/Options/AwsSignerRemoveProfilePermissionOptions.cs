using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("signer", "remove-profile-permission")]
public record AwsSignerRemoveProfilePermissionOptions(
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--revision-id")] string RevisionId,
[property: CommandSwitch("--statement-id")] string StatementId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}