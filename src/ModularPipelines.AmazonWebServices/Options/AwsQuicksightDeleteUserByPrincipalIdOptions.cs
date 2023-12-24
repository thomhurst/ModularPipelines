using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "delete-user-by-principal-id")]
public record AwsQuicksightDeleteUserByPrincipalIdOptions(
[property: CommandSwitch("--principal-id")] string PrincipalId,
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--namespace")] string Namespace
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}