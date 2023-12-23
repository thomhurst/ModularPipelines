using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudcontrol", "update-resource")]
public record AwsCloudcontrolUpdateResourceOptions(
[property: CommandSwitch("--type-name")] string TypeName,
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--patch-document")] string PatchDocument
) : AwsOptions
{
    [CommandSwitch("--type-version-id")]
    public string? TypeVersionId { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}