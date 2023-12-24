using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databrew", "update-project")]
public record AwsDatabrewUpdateProjectOptions(
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--sample")]
    public string? Sample { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}