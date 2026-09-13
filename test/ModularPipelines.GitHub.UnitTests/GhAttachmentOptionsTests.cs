using ModularPipelines.Context;
using ModularPipelines.GitHub.Options;
using ModularPipelines.TestHelpers;
using TUnit.Assertions.Enums;

namespace ModularPipelines.GitHub.UnitTests;

public class GhAttachmentOptionsTests : TestBase
{
    [Test]
    [Arguments("issue create")]
    [Arguments("issue edit")]
    [Arguments("issue comment")]
    [Arguments("pr create")]
    [Arguments("pr edit")]
    [Arguments("pr comment")]
    public async Task Renders_Each_Attachment_As_A_Separate_Option(string command)
    {
        string[] attachments = ["./before image.png#Before, with alt text", "./after.png#After"];
        GhOptions options = command switch
        {
            "issue create" => new GhIssueCreateOptions { Attach = attachments },
            "issue edit" => new GhIssueEditOptions(["123"]) { Attach = attachments },
            "issue comment" => new GhIssueCommentOptions("123") { Attach = attachments },
            "pr create" => new GhPrCreateOptions { Attach = attachments },
            "pr edit" => new GhPrEditOptions { Attach = attachments },
            "pr comment" => new GhPrCommentOptions { Attach = attachments },
            _ => throw new ArgumentOutOfRangeException(nameof(command)),
        };
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(options);

        await Assert.That(commandLine.Arguments.Where(argument => argument.StartsWith("--attach=", StringComparison.Ordinal)))
            .IsEquivalentTo(
                ["--attach=./before image.png#Before, with alt text", "--attach=./after.png#After"],
                CollectionOrdering.Matching);
    }
}
