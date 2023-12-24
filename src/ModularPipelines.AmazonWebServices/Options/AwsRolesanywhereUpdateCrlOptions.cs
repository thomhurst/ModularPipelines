using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rolesanywhere", "update-crl")]
public record AwsRolesanywhereUpdateCrlOptions(
[property: CommandSwitch("--crl-id")] string CrlId
) : AwsOptions
{
    [CommandSwitch("--crl-data")]
    public string? CrlData { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}