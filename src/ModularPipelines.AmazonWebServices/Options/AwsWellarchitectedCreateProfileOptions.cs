using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "create-profile")]
public record AwsWellarchitectedCreateProfileOptions(
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--profile-description")] string ProfileDescription,
[property: CommandSwitch("--profile-questions")] string[] ProfileQuestions
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}