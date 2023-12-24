using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "create-retriever")]
public record AwsQbusinessCreateRetrieverOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--configuration")] string Configuration,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}