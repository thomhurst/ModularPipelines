using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class GcloudSynopsisGroupReconcilerTests
{
    [Test]
    [Arguments("tool run [--first=FIRST | --second=SECOND]")]
    [Arguments("tool run (--first=FIRST : --selector=SELECTOR | --second=SECOND)")]
    [Arguments("tool run (--first=FIRST | OPERAND)")]
    [Arguments("tool run (--first=FIRST; default=one | --second=SECOND)")]
    public async Task Required_Option_Constraints_Exclude_Optional_Ambiguous_And_Operand_Syntax(string synopsis)
    {
        await Assert.That(UsageSynopsisParser.GetRequiredOptionChoiceGroups(synopsis)).IsEmpty();
    }

    [Test]
    public async Task Required_Option_Constraints_Preserve_Nested_Choices_And_Optional_Selectors()
    {
        var group = UsageSynopsisParser.GetRequiredOptionChoiceGroups(
            "tool run ((--first=FIRST | --second=SECOND) (--file=FILE | --prefix=PREFIX) | [--resource=RESOURCE : --selector=SELECTOR])").Single();
        await Assert.That(group.IsChoice).IsTrue();
        await Assert.That(group.IsRequired).IsTrue();
        await Assert.That(group.Groups[0].Groups.Count).IsEqualTo(2);
        await Assert.That(group.Groups[0].Groups.All(choice => choice.IsChoice && choice.IsRequired)).IsTrue();
        var resource = group.Groups[1];
        await Assert.That(resource.IsRequired).IsFalse();
        await Assert.That(resource.Members[0].IsRequired).IsTrue();
        await Assert.That(resource.Members[1].IsRequired).IsFalse();
    }

    [Test]
    public async Task Resource_Bundles_Identify_Their_Single_Primary_Option()
    {
        var bundles = UsageSynopsisParser.GetOptionBundles(
            "tool run [--key=KEY : --keyring=RING] [--host=HOST --secret=SECRET : --certificate=CERT] [--host=HOST : --token=TOKEN | --password=PASSWORD]").ToArray();
        await Assert.That(bundles.Length).IsEqualTo(2);
        await Assert.That(bundles[0].PrimarySwitch).IsEqualTo("--key");
        await Assert.That(bundles[0].OptionSwitches).IsEquivalentTo(["--key", "--keyring"]);
        await Assert.That(bundles[0].DirectOptionalSwitches).IsEquivalentTo(["--keyring"]);
        await Assert.That(bundles[1].PrimarySwitch).IsNull();
        await Assert.That(bundles[1].DirectOptionalSwitches).IsEquivalentTo(["--certificate"]);
    }

    [Test]
    public async Task Provider_Selectors_Do_Not_Join_A_Different_Credential_Resource()
    {
        var group = Choice("--other", "--host", "--certificate") with
        {
            Groups =
            [
                new()
                {
                    Kind = CliArgumentGroupKind.Resource,
                    Arguments = [new() { SwitchName = "--secret" }],
                },
            ],
        };
        var result = Reconcile(group, "[--other=OTHER | [--host=HOST : --secret=SECRET --certificate=CERT]]");
        var branch = result.Groups.Single();
        await Assert.That(branch.Arguments.Select(argument => argument.SwitchName)).IsEquivalentTo(["--host", "--certificate"]);
        await Assert.That(branch.Groups.Single().Arguments.Select(argument => argument.SwitchName)).IsEquivalentTo(["--secret"]);
    }

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
        GcloudSynopsisGroupReconciler.Reconcile(group, UsageSynopsisParser.GetOptionChoiceBranches(synopsis).ToArray(),
            UsageSynopsisParser.GetOptionBundles(synopsis).ToArray());
}
