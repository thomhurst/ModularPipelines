using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fms", "put-admin-account")]
public record AwsFmsPutAdminAccountOptions(
[property: CommandSwitch("--admin-account")] string AdminAccount
) : AwsOptions
{
    [CommandSwitch("--admin-scope")]
    public string? AdminScope { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}