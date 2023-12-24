using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "create-login-profile")]
public record AwsIamCreateLoginProfileOptions(
[property: CommandSwitch("--user-name")] string UserName,
[property: CommandSwitch("--password")] string Password
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}