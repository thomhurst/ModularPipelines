using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "list-grants")]
public record AwsKmsListGrantsOptions(
[property: CommandSwitch("--key-id")] string KeyId
) : AwsOptions
{
    [CommandSwitch("--grant-id")]
    public string? GrantId { get; set; }

    [CommandSwitch("--grantee-principal")]
    public string? GranteePrincipal { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}