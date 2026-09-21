using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class GcloudSynopsisGroupReconcilerTests
{
    [Test]
    public async Task Explicit_Resource_Branches_Keep_Their_Selectors_Together()
    {
        var group = Choice("--image", "--tag", "--environment", "--location");
        var result = Reconcile(group, "[[--image=IMAGE : --tag=TAG] | [--environment=ENV : --location=LOCATION]]");
        await Assert.That(result.Arguments).IsEmpty();
        await Assert.That(result.Groups.Select(branch => string.Join(",", branch.FlattenArguments().Select(argument => argument.SwitchName))))
            .IsEquivalentTo(["--image,--tag", "--environment,--location"]);
    }

    [Test]
    [Arguments("[--image=IMAGE | --environment=ENV]")]
    [Arguments("[--image=IMAGE | --tag=TAG | --environment=ENV | --location=LOCATION | --extra=EXTRA]")]
    [Arguments("[--image=IMAGE : --tag=TAG | --environment=ENV --location=LOCATION]")]
    [Arguments("[--image=IMAGE --tag=TAG | --image=IMAGE --environment=ENV --location=LOCATION]")]
    [Arguments("[--image=IMAGE --tag=TAG --environment=ENV --location=LOCATION | OPERAND]")]
    public async Task Partial_Overlapping_Or_Common_Selector_Syntax_Does_Not_Replace_Documented_Groups(string synopsis)
    {
        var group = Choice("--image", "--tag", "--environment", "--location");
        var result = Reconcile(group, synopsis);
        await Assert.That(result.Arguments).IsEquivalentTo(group.Arguments);
        await Assert.That(result.Groups).IsEmpty();
    }

    private static CliArgumentGroup Choice(params string[] switches) => new()
    {
        Kind = CliArgumentGroupKind.AtMostOne,
        Arguments = [.. switches.Select(name => new CliArgumentDefinition { SwitchName = name })],
    };

    private static CliArgumentGroup Reconcile(CliArgumentGroup group, string synopsis) =>
        GcloudSynopsisGroupReconciler.Reconcile(group, UsageSynopsisParser.GetOptionChoiceBranches(synopsis).ToArray());
}
