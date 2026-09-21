using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class UsageSynopsisParserTests
{
    [Test]
    public async Task Optional_Common_Selectors_Remain_Independent_Of_Nested_Alternatives()
    {
        var documentedGroup = new CliArgumentGroup
        {
            Arguments =
            [
                new() { SwitchName = "--host" },
                new() { SwitchName = "--directory" },
                new() { SwitchName = "--certificate" },
            ],
            Groups =
            [
                new()
                {
                    Kind = CliArgumentGroupKind.AtMostOne,
                    Groups =
                    [
                        new() { Arguments = [new() { SwitchName = "--token" }] },
                        new() { Arguments = [new() { SwitchName = "--username" }, new() { SwitchName = "--password" }] },
                    ],
                },
            ],
        };
        var groups = UsageSynopsisParser.GetOptionalResourceOptionGroups(
            "tool run [--host=HOST : --directory=DIR --certificate=CERT --token=TOKEN | [--username=USER : --password=PASSWORD]]",
            [documentedGroup]).ToArray();

        await Assert.That(groups.Any(group => group.SetEquals(["--directory"]))).IsTrue();
        await Assert.That(groups.Any(group => group.SetEquals(["--certificate"]))).IsTrue();
        await Assert.That(groups.Any(group => group.SetEquals(["--token"]))).IsTrue();
        await Assert.That(groups.Any(group => group.SetEquals(["--username", "--password"]))).IsTrue();
        await Assert.That(groups.Any(group => group.SetEquals(["--password"]))).IsTrue();
        await Assert.That(groups.Any(group => group.SetEquals(["--directory", "--certificate", "--token"]))).IsFalse();
    }

    [Test]
    public async Task Optional_Selectors_Preserve_Whole_Alternative_Branches()
    {
        var groups = UsageSynopsisParser.GetOptionalResourceOptionGroups(
            "tool run [--kind=KIND : --id=ID --secret=SECRET | --user=USER --password=PASSWORD]")
            .ToArray();
        await Assert.That(groups).Count().IsEqualTo(3);
        await Assert.That(groups[1]).IsEquivalentTo(["--id", "--secret"]);
        await Assert.That(groups[2]).IsEquivalentTo(["--user", "--password"]);
    }

    [Test]
    public async Task Alternative_Branches_Preserve_Explicitly_Optional_Nested_Resources()
    {
        var groups = UsageSynopsisParser.GetOptionalResourceOptionGroups(
            "tool run [--kind=KIND : --id=ID [--secret=SECRET : --secret-project=PROJECT] | --user=USER]")
            .ToArray();
        await Assert.That(groups.Any(group => group.SetEquals(["--id", "--secret", "--secret-project"]))).IsTrue();
        await Assert.That(groups.Any(group => group.SetEquals(["--secret", "--secret-project"]))).IsTrue();
        await Assert.That(groups.Any(group => group.SetEquals(["--secret-project"]))).IsTrue();
    }

    [Test]
    [Arguments("(RESOURCE --parent=PARENT)")]
    [Arguments("(RESOURCE --parent=PARENT [--optional=VALUE])")]
    [Arguments("((RESOURCE --parent=PARENT) [--optional=VALUE])")]
    [Arguments("((RESOURCE --parent=PARENT) : --selector=VALUE)")]
    public async Task Required_Group_Preserves_Required_Options(string group)
    {
        var result = UsageSynopsisParser.Parse($"Usage: tool run {group}", ["tool", "run"]);
        await Assert.That(result.RequiredOptionSwitches).IsEquivalentTo(["--parent"]);
        await Assert.That(result.PositionalArguments.Single().IsRequired).IsTrue();
    }

    [Test]
    public async Task Optional_Group_Does_Not_Discard_Conditional_Requirements()
    {
        var result = UsageSynopsisParser.Parse("Usage: tool run [RESOURCE --parent=PARENT]", ["tool", "run"]);
        var group = result.RequiredAlternativeGroups.Single();
        await Assert.That(group.IsRequired).IsFalse();
        await Assert.That(group.IsChoice).IsFalse();
        await Assert.That(group.Members.Select(member => (member.OptionSwitch ?? member.PositionalPropertyName)!))
            .IsEquivalentTo(["Resource", "--parent"]);
        await Assert.That(group.Members.All(member => member.IsRequired)).IsTrue();
    }

    [Test]
    [Arguments("(RESOURCE [CHILD --parent=PARENT])")]
    [Arguments("[RESOURCE [CHILD --parent=PARENT]]")]
    public async Task Nested_Optional_Bundles_Retain_Conditional_Requirements(string syntax)
    {
        var result = UsageSynopsisParser.Parse($"Usage: tool run {syntax}", ["tool", "run"]);
        var outer = result.RequiredAlternativeGroups.Single();
        var group = outer.Groups.Count > 0 ? outer.Groups.Single() : outer;
        await Assert.That(group.IsRequired).IsFalse();
        await Assert.That(group.IsChoice).IsFalse();
        await Assert.That(group.Members.Select(member => (member.OptionSwitch ?? member.PositionalPropertyName)!))
            .IsEquivalentTo(["Child", "--parent"]);
        await Assert.That(group.Members.All(member => member.IsRequired)).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Optional_Bundle_Does_Not_Constrain_An_Alternate_Form(bool reverse)
    {
        string[] forms = ["tool run [RESOURCE --parent=PARENT]", "tool run RESOURCE"];
        if (reverse)
        {
            Array.Reverse(forms);
        }
        var usage = UsageSynopsisParser.Parse("Usage: " + string.Join("\n       ", forms), ["tool", "run"]);
        await Assert.That(usage.RequiredAlternativeGroups).IsEmpty();
        var resolved = UsageSynopsisParser.ResolveOptionUsage(usage,
            [new() { SwitchName = "--parent", PropertyName = "Parent", CSharpType = "string?" }]);
        await Assert.That(resolved.RequiredAlternativeGroups).IsEmpty();
    }

    [Test]
    [Arguments("[RESOURCE --parent=PARENT]")]
    [Arguments("[RESOURCE [CHILD --parent=PARENT]]")]
    public async Task Common_Optional_Bundles_Survive_Alternate_Forms(string bundle)
    {
        var usage = UsageSynopsisParser.Parse(
            $"Usage: tool run {bundle}\n       tool run {bundle} [--quiet]", ["tool", "run"]);
        await Assert.That(usage.RequiredAlternativeGroups.Count).IsEqualTo(1);
        var resolved = UsageSynopsisParser.ResolveOptionUsage(usage,
            [new() { SwitchName = "--parent", PropertyName = "Parent", CSharpType = "string?" },
             new() { SwitchName = "--quiet", PropertyName = "Quiet", CSharpType = "bool?", IsFlag = true }]);
        await Assert.That(resolved.RequiredAlternativeGroups.Count).IsEqualTo(1);
    }

    [Test]
    public async Task Optional_Switch_Activates_Its_Enclosing_Resource_Bundle()
    {
        var usage = UsageSynopsisParser.Parse("Usage: tool run [RESOURCE [--format=FORMAT]]", ["tool", "run"]);
        var bundle = usage.RequiredAlternativeGroups.Single();
        await Assert.That(bundle.IsRequired).IsFalse();
        await Assert.That(bundle.Members.Single(member => member.PositionalPropertyName == "Resource").IsRequired).IsTrue();
        await Assert.That(bundle.Members.Single(member => member.OptionSwitch == "--format").IsRequired).IsFalse();
    }

    [Test]
    public async Task Optional_Flag_Bundle_Is_Resolved_After_Flag_Shapes_Are_Known()
    {
        var usage = UsageSynopsisParser.Parse("Usage: tool run [--verbose RESOURCE]", ["tool", "run"]);
        var result = UsageSynopsisParser.ResolveOptionUsage(usage,
            [new() { SwitchName = "--verbose", PropertyName = "Verbose", CSharpType = "bool?", IsFlag = true }]);
        var group = result.RequiredAlternativeGroups.Single();
        await Assert.That(group.IsRequired).IsFalse();
        await Assert.That(group.IsChoice).IsFalse();
        await Assert.That(group.Members.Select(member => (member.OptionSwitch ?? member.PositionalPropertyName)!))
            .IsEquivalentTo(["Resource", "--verbose"]);
    }

    [Test]
    public async Task Nested_Required_Option_Choice_Does_Not_Require_Both_Options()
    {
        var result = UsageSynopsisParser.Parse("Usage: tool run (RESOURCE (--a=A | --b=B))", ["tool", "run"]);
        await Assert.That(result.RequiredOptionSwitches).IsEmpty();
        var group = result.RequiredAlternativeGroups.Single();
        await Assert.That(group.IsChoice).IsTrue();
        await Assert.That(group.Members.Select(member => member.OptionSwitch!)).IsEquivalentTo(["--a", "--b"]);
    }

    [Test]
    public async Task Optional_Bundle_Keeps_Nested_Choice_Conditional()
    {
        var result = UsageSynopsisParser.Parse("Usage: tool run [RESOURCE (--a=A | --b=B)]", ["tool", "run"]);
        var bundle = result.RequiredAlternativeGroups.Single();
        await Assert.That(bundle.IsRequired).IsFalse();
        await Assert.That(bundle.IsChoice).IsFalse();
        await Assert.That(bundle.Members.Single().PositionalPropertyName).IsEqualTo("Resource");
        var choice = bundle.Groups.Single();
        await Assert.That(choice.IsRequired).IsTrue();
        await Assert.That(choice.IsChoice).IsTrue();
        await Assert.That(choice.Members.Select(member => member.OptionSwitch!)).IsEquivalentTo(["--a", "--b"]);
    }

    [Test]
    [Arguments("((--a=A --x=X) : --b=B)")]
    [Arguments("((--a=A|--x=X):--b=B)")]
    public async Task Required_Option_Only_Nested_Colon_Group_Is_Not_Discarded(string group)
    {
        await Assert.That(() => UsageSynopsisParser.Parse(
                $"Usage: tool run {group}", ["tool", "run"]))
            .Throws<InvalidOperationException>();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Documented_Option_Groups_Preserve_Operands_And_Line_Boundaries(bool compact)
    {
        var group = compact ? "((--a=A|--b=B):--c=C)" : "((--a=A | --b=B) : --c=C)";
        var synopsis = $"    tool run RESOURCE {group} --required=VALUE\n\n";
        var normalized = UsageSynopsisParser.DeferDocumentedOptionGroups(synopsis,
        [
            new CliArgumentGroup
            {
                Kind = CliArgumentGroupKind.AtLeastOne,
                Arguments = [new() { SwitchName = "--c" }],
                Groups = [new() { Arguments = [new() { SwitchName = "--a" }, new() { SwitchName = "--b" }] }],
            },
        ]);
        var usage = UsageSynopsisParser.Parse("Usage:\n" + normalized, ["tool", "run"]);
        await Assert.That(normalized).EndsWith("\n\n");
        await Assert.That(normalized).DoesNotContain("--a");
        await Assert.That(usage.PositionalArguments.Single().PropertyName).IsEqualTo("Resource");
        await Assert.That(usage.RequiredOptionSwitches).IsEquivalentTo(["--required"]);
        await Assert.That(usage.UnparsedOperandTokens).IsEmpty();
    }

    [Test]
    [Arguments(CliArgumentGroupKind.None, false, false)]
    [Arguments(CliArgumentGroupKind.AtMostOne, false, false)]
    [Arguments(CliArgumentGroupKind.AtLeastOne, true, false)]
    [Arguments(CliArgumentGroupKind.AtLeastOne, false, true)]
    public async Task Undocumented_Or_Mixed_Option_Groups_Are_Not_Deferred(
        CliArgumentGroupKind kind, bool missingOption, bool positional)
    {
        const string synopsis = "tool run ((--a=A --x=X) : --b=B)";
        var normalized = UsageSynopsisParser.DeferDocumentedOptionGroups(synopsis,
        [
            new CliArgumentGroup
            {
                Kind = kind,
                Arguments =
                [
                    new() { SwitchName = "--a", IsPositional = positional },
                    new() { SwitchName = missingOption ? "--other" : "--x" },
                    new() { SwitchName = "--b" },
                ],
            },
        ]);
        await Assert.That(normalized).IsEqualTo(synopsis);
        await Assert.That(() => UsageSynopsisParser.Parse("Usage: " + normalized, ["tool", "run"]))
            .Throws<InvalidOperationException>();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Deferral_Preserves_Identical_Text_Inside_Unmatched_Groups(bool nestedFirst)
    {
        const string group = "((--a=A|--b=B):--c=C)";
        var nested = $"(RESOURCE | {group})";
        var synopsis = nestedFirst ? $"tool run {nested}\n {group}\n" : $"tool run {group}\n {nested}\n";
        var normalized = UsageSynopsisParser.DeferDocumentedOptionGroups(synopsis,
        [
            new CliArgumentGroup
            {
                Kind = CliArgumentGroupKind.AtLeastOne,
                Arguments = [new() { SwitchName = "--a" }, new() { SwitchName = "--b" }, new() { SwitchName = "--c" }],
            },
        ]);
        await Assert.That(normalized).IsEqualTo(nestedFirst ? $"tool run {nested}\n  \n" : $"tool run  \n {nested}\n");
    }

    [Test]
    public async Task Duplicate_Documented_Option_Sets_Are_Not_Deferred()
    {
        const string synopsis = "tool run ((--a=A --x=X) : --b=B)";
        var group = new CliArgumentGroup
        {
            Kind = CliArgumentGroupKind.AtLeastOne,
            Arguments = [new() { SwitchName = "--a" }, new() { SwitchName = "--x" }, new() { SwitchName = "--b" }],
        };
        var normalized = UsageSynopsisParser.DeferDocumentedOptionGroups(synopsis,
            [group, group with { Kind = CliArgumentGroupKind.AtLeastOne | CliArgumentGroupKind.AtMostOne }]);
        await Assert.That(normalized).IsEqualTo(synopsis);
        await Assert.That(() => UsageSynopsisParser.Parse("Usage: " + normalized, ["tool", "run"]))
            .Throws<InvalidOperationException>();
    }

    [Test]
    [Arguments("[USER@]INSTANCE", "UserInstance", true, false)]
    [Arguments("[[USER@]INSTANCE:]SRC", "UserInstanceSrc", true, false)]
    [Arguments("[[[USER@]INSTANCE:]SRC ...]", "UserInstanceSrc", false, true)]
    [Arguments("(cloudshell|localhost):SRC", "CloudshellLocalhostSrc", true, false)]
    [Arguments("[(cloudshell|localhost):SRC ...]", "CloudshellLocalhostSrc", false, true)]
    [Arguments("[-- ARGS ...]", "Args", false, true)]
    [Arguments("(RESOURCE --parent=PARENT)", "Resource", true, false)]
    public async Task Compound_And_Grouped_Operands_Retain_Their_Value_Contract(
        string syntax, string name, bool required, bool variadic)
    {
        var result = UsageSynopsisParser.Parse($"Usage: tool run {syntax}", ["tool", "run"]);
        var operand = result.PositionalArguments.Single();
        await Assert.That(operand.PropertyName).IsEqualTo(name);
        await Assert.That(operand.IsRequired).IsEqualTo(required);
        await Assert.That(operand.IsVariadic).IsEqualTo(variadic);
        await Assert.That(result.UnparsedOperandTokens).IsEmpty();
    }

    [Test]
    [Arguments("[(--spark-main-class=CLASS | --spark-main-jar-file-uri=JAR) : --vpc-network-name=NETWORK | --vpc-sub-network-name=SUBNET]")]
    [Arguments("[(--spark-main-class=CLASS | --spark-main-jar-file-uri=JAR) : --packages=[PACKAGES, ...] --vpc-network-name=NETWORK | --vpc-sub-network-name=SUBNET]")]
    [Arguments("[(--spark-main-class=CLASS|--spark-main-jar-file-uri=JAR) : --vpc-network-name=NETWORK|--vpc-sub-network-name=SUBNET]")]
    public async Task Option_Only_Colon_Groups_Do_Not_Create_Operands(string group)
    {
        var result = UsageSynopsisParser.Parse($"Usage: tool run RESOURCE {group}", ["tool", "run"]);

        var operand = result.PositionalArguments.Single();
        await Assert.That(operand.PropertyName).IsEqualTo("Resource");
        await Assert.That(operand.IsRequired).IsTrue();
        await Assert.That(result.UnparsedOperandTokens).IsEmpty();
        await Assert.That(result.RequiredOptionSwitches).IsEmpty();
    }

    [Test]
    [Arguments("-- [(--a|--b) : --c|--d]", true)]
    [Arguments("-- [(--a|--b) : --c|--d] [--e : --f]", true)]
    [Arguments("[(--a|--b) : --c|--d]", false)]
    public async Task Option_Only_Groups_Preserve_Pending_Option_Terminator(string prefix, bool prependTerminator)
    {
        var result = UsageSynopsisParser.Parse($"Usage: tool run {prefix} FILE", ["tool", "run"]);

        var operand = result.PositionalArguments.Single();
        await Assert.That(operand.PropertyName).IsEqualTo("File");
        await Assert.That(operand.PrependOptionTerminator).IsEqualTo(prependTerminator);
        await Assert.That(result.UnparsedOperandTokens).IsEmpty();
    }

    [Test]
    public async Task Repeated_Adapter_Operand_Names_Do_Not_Turn_Required_Slots_Into_Choices()
    {
        var usage = UsageSynopsisParser.Parse("""
            Usage:
              tool run <source> <destination>
              tool run <path> <other> --all
            """, ["tool", "run"]);
        // An adapter may normalize display names without merging distinct operand slots.
        usage = usage with
        {
            RequirednessCandidates = [.. usage.RequirednessCandidates.Select(candidate =>
                candidate.Synopsis!.Contains("<path>", StringComparison.Ordinal)
                    ? candidate with
                    {
                        PositionalArguments = [.. candidate.PositionalArguments.Select(argument =>
                            argument with { PropertyName = "Path" })],
                    }
                    : candidate)],
        };

        var resolved = UsageSynopsisParser.ResolveOptionUsage(usage,
            [new() { SwitchName = "--all", PropertyName = "All", CSharpType = "bool?", IsFlag = true }]);
        await Assert.That(resolved.RequiredAlternativeGroups).IsEmpty();
        await Assert.That(resolved.PositionalArguments.All(argument => argument.IsRequired)).IsTrue();
    }

    [Test]
    [Arguments("(RESOURCE|ALIAS : --location=LOCATION)")]
    [Arguments("[RESOURCE | ALIAS : --location=LOCATION]")]
    [Arguments("(SOURCE : DESTINATION | ALTERNATIVE)")]
    [Arguments("(RESOURCE : --location=LOCATION | --global)")]
    [Arguments("((--input=INPUT | --other=OTHER) : TARGET | ALTERNATIVE)")]
    [Arguments("((--input VALUE | --other=OTHER) : --network=NETWORK | --global)")]
    [Arguments("((--input=INPUT | --other=OTHER) : --network=NETWORK | --global)")]
    [Arguments("[--force|TARGET : --location=LOCATION]")]
    [Arguments("[--force | TARGET : --location=LOCATION]")]
    [Arguments("[TARGET|--force : --location=LOCATION]")]
    public async Task Rejects_Ambiguous_Alternatives_Across_Colon_Groups(string group)
    {
        await Assert.That(() => UsageSynopsisParser.Parse(
                $"Usage: tool show {group}", ["tool", "show"]))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("ambiguous alternatives in colon group");
    }

    [Test]
    [Arguments("((RESOURCE|ALIAS) : --location=LOCATION)")]
    [Arguments("(RESOURCE : --location=(REGION|ZONE))")]
    public async Task Colon_Groups_Preserve_Explicitly_Nested_Alternatives(string group)
    {
        var result = UsageSynopsisParser.Parse($"Usage: tool show {group}", ["tool", "show"]);

        var operand = result.PositionalArguments.Single();
        await Assert.That(operand.PropertyName).IsEqualTo("Resource");
        await Assert.That(operand.IsRequired).IsTrue();
    }

    [Test]
    [Arguments("(RESOURCE : --location=LOCATION)", true)]
    [Arguments("[RESOURCE : --location=LOCATION]", false)]
    [Arguments("(RESOURCE\n          : --location=LOCATION --service=SERVICE)", true)]
    public async Task Joins_Indented_Synopsis_Continuation_Lines(string resourceGroup, bool required)
    {
        var result = UsageSynopsisParser.Parse(
            $"SYNOPSIS\n    gcloud example show\n        {resourceGroup}\n        [--async]\nDESCRIPTION\n    Show a resource.",
            ["gcloud", "example", "show"], acceptedHeadings: ["SYNOPSIS"]);

        var operand = result.PositionalArguments.Single();
        await Assert.That(operand.PropertyName).IsEqualTo("Resource");
        await Assert.That(operand.IsRequired).IsEqualTo(required);
        await Assert.That(result.Synopsis).Contains("[--async]").And.DoesNotContain("DESCRIPTION");
    }

    [Test]
    public async Task Indented_Synopsis_Alternatives_Remain_Separate()
    {
        const string help = """
            SYNOPSIS
                tool show
                    RESOURCE
                tool show
                    --all
            DESCRIPTION
                Show one or all resources.
            """;
        var result = UsageSynopsisParser.Parse(help, ["tool", "show"], acceptedHeadings: ["SYNOPSIS"]);

        await Assert.That(result.MatchedSynopsisCount).IsEqualTo(2);
        await Assert.That(result.PositionalArguments.Single().PropertyName).IsEqualTo("Resource");
        await Assert.That(result.PositionalArguments.Single().IsRequired).IsFalse();
    }

    [Test]
    [Arguments("(RESOURCE : --location=LOCATION)", true, false)]
    [Arguments("[RESOURCE : --location=LOCATION]", false, false)]
    [Arguments("(RESOURCE [RESOURCE ...] : --location=LOCATION)", true, true)]
    [Arguments("[RESOURCE ... : --location=LOCATION]", false, true)]
    [Arguments("(RESOURCE : --location=LOCATION --service=SERVICE)", true, false)]
    [Arguments("(RESOURCE : [--location=LOCATION])", true, false)]
    public async Task Resource_Groups_Keep_Only_Their_Positional_Inputs(
        string resourceGroup, bool required, bool variadic)
    {
        var result = UsageSynopsisParser.Parse(
            $"SYNOPSIS\n    gcloud example show {resourceGroup}",
            ["gcloud", "example", "show"], acceptedHeadings: ["SYNOPSIS"]);
        var operand = result.PositionalArguments.Single();

        await Assert.That(operand.PropertyName).IsEqualTo("Resource");
        await Assert.That(operand.IsRequired).IsEqualTo(required);
        await Assert.That(operand.IsVariadic).IsEqualTo(variadic);
        await Assert.That(operand.AssociatedOptionSwitch).IsNull();
    }

    [Test]
    [Arguments("(SOURCE : DESTINATION)", true, true, CommandLinePhase.EarlyOperand, 1)]
    [Arguments("[SOURCE : DESTINATION]", false, false, CommandLinePhase.EarlyOperand, 1)]
    [Arguments("(SOURCE : [DESTINATION])", true, false, CommandLinePhase.EarlyOperand, 1)]
    [Arguments("(SOURCE : --location=LOCATION DESTINATION)", true, true, CommandLinePhase.Passthrough, 0)]
    public async Task Colon_Groups_Preserve_Operands_After_The_Separator(
        string group, bool sourceRequired, bool destinationRequired,
        CommandLinePhase destinationPhase, int destinationIndex)
    {
        var result = UsageSynopsisParser.Parse($"Usage: tool copy {group}", ["tool", "copy"]);

        await Assert.That(result.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["Source", "Destination"]);
        await Assert.That(result.PositionalArguments[0].PositionIndex).IsEqualTo(0);
        await Assert.That(result.PositionalArguments[1].PositionIndex).IsEqualTo(destinationIndex);
        await Assert.That(result.PositionalArguments[1].Phase).IsEqualTo(destinationPhase);
        await Assert.That(result.PositionalArguments[0].IsRequired).IsEqualTo(sourceRequired);
        await Assert.That(result.PositionalArguments[1].IsRequired).IsEqualTo(destinationRequired);
        await Assert.That(result.PositionalArguments[1].AssociatedOptionSwitch).IsNull();
    }

    [Test]
    [Arguments("(--verbose <TARGET>|--file <FILE>)", true, false)]
    [Arguments("(--file <FILE>|--verbose <TARGET>)", true, false)]
    [Arguments("(--verbose <TARGET>|--file <FILE>)", false, true)]
    [Arguments("(--verbose|--file <FILE>)", true, true)]
    public async Task Resolving_Single_Synopsis_Uses_Option_Shapes_For_Inline_Alternatives(
        string alternatives, bool verboseIsFlag, bool hasAlternativeGroup)
    {
        var usage = UsageSynopsisParser.Parse($"Usage: tool run {alternatives}", ["tool", "run"]);
        CliOptionDefinition[] options =
        [
            new() { SwitchName = "--verbose", PropertyName = "Verbose", CSharpType = verboseIsFlag ? "bool?" : "string?", IsFlag = verboseIsFlag },
            new() { SwitchName = "--file", PropertyName = "File", CSharpType = "string?" },
        ];

        var resolved = UsageSynopsisParser.ResolveOptionUsage(usage, options);
        var repeated = UsageSynopsisParser.ResolveOptionUsage(resolved, options);

        await Assert.That(resolved.RequiredAlternativeGroups.Count).IsEqualTo(hasAlternativeGroup ? 1 : 0);
        await Assert.That(repeated.RequiredAlternativeGroups).IsEquivalentTo(resolved.RequiredAlternativeGroups);
        if (hasAlternativeGroup)
        {
            await Assert.That(resolved.RequiredAlternativeGroups.Single().Members.Select(member => member.OptionSwitch!))
                .IsEquivalentTo(["--verbose", "--file"]);
        }
    }

    [Test]
    public async Task Resolving_Alternatives_Reranks_After_Removing_The_Only_Selected_Placeholder()
    {
        var usage = UsageSynopsisParser.Parse(
            "Usage: tool group <COMMAND>\n       tool group <TARGET>", ["tool", "group"]);
        var normalized = UsageSynopsisParser.RemoveCommandGroupPlaceholders(usage);

        var resolved = UsageSynopsisParser.ResolveOptionUsage(normalized, []);

        await Assert.That(resolved.PositionalArguments.Single().PropertyName).IsEqualTo("Target");
        await Assert.That(resolved.PositionalArguments.Single().IsRequired).IsFalse();
        await Assert.That(resolved.HasOperandTokens).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Resolving_Flag_Shapes_Does_Not_Flatten_A_Conjunction_Into_An_Alternative(bool fileFirst)
    {
        var flagForm = "tool run --verbose <TARGET>";
        var fileForm = "tool run --file <FILE>";
        var usage = UsageSynopsisParser.Parse(
            $"Usage: {(fileFirst ? fileForm : flagForm)}\n       {(fileFirst ? flagForm : fileForm)}", ["tool", "run"]);
        CliOptionDefinition[] options =
        [
            new() { SwitchName = "--verbose", PropertyName = "Verbose", CSharpType = "bool?", IsFlag = true },
            new() { SwitchName = "--file", PropertyName = "File", CSharpType = "string?" },
        ];

        var resolved = UsageSynopsisParser.ResolveOptionUsage(usage, options);
        var repeated = UsageSynopsisParser.ResolveOptionUsage(resolved, options);

        foreach (var result in new[] { resolved, repeated })
        {
            var group = result.RequiredAlternativeGroups.Single();
            await Assert.That(group.Members.Single().OptionSwitch).IsEqualTo("--file");
            var bundle = group.Groups.Single();
            await Assert.That(bundle.IsChoice).IsFalse();
            await Assert.That(bundle.Members.Select(member => (member.OptionSwitch ?? member.PositionalPropertyName)!))
                .IsEquivalentTo(["--verbose", "Target"]);
        }
        await Assert.That(resolved.PositionalArguments.Single().PropertyName).IsEqualTo("Target");
        await Assert.That(resolved.PositionalArguments.Single().IsRequired).IsFalse();
    }

    [Test]
    public async Task Resolving_Alternatives_Does_Not_Restore_Renamed_Command_Group_Placeholders()
    {
        var usage = UsageSynopsisParser.Parse(
            "Usage: tool group <COMMAND> <ARG>\n       tool group <SUBCOMMAND> <ARG>",
            ["tool", "group"]);
        var normalized = UsageSynopsisParser.RemoveCommandGroupPlaceholders(usage);

        var resolved = UsageSynopsisParser.ResolveOptionUsage(normalized, []);

        await Assert.That(resolved.PositionalArguments.Single().PropertyName).IsEqualTo("Arg");
        await Assert.That(resolved.RequirednessCandidates.SelectMany(candidate => candidate.PositionalArguments)
            .Any(UsageSynopsisParser.IsCommandGroupPlaceholder)).IsFalse();
    }

    [Test]
    public async Task Case_Distinct_Value_Switch_Does_Not_Borrow_A_Flag_Shape()
    {
        var usage = UsageSynopsisParser.Parse("Usage: tool run -F <VALUE>", ["tool", "run"]);
        CliOptionDefinition[] options =
        [
            new() { SwitchName = "-f", PropertyName = "Force", CSharpType = "bool?", IsFlag = true },
            new() { SwitchName = "-F", PropertyName = "File", CSharpType = "string?" },
        ];

        await Assert.That(UsageSynopsisParser.IsPositionalSlot(usage.PositionalArguments.Single(), options)).IsFalse();
    }

    [Test]
    public async Task Reranking_Does_Not_Restore_Removed_Option_Values_With_A_Positional_Name()
    {
        var usage = UsageSynopsisParser.Parse("Usage: tool run [OPTIONS] --file <PATH> --config <CONFIG> <PATH>\n       tool run [OPTIONS] --file <PATH> <X> <Y>", ["tool", "run"]);
        CliOptionDefinition[] options =
        [
            new() { SwitchName = "--file", PropertyName = "File", CSharpType = "string?" },
            new() { SwitchName = "--config", PropertyName = "Config", CSharpType = "string?" },
        ];
        var normalized = usage with
        {
            PositionalArguments = [.. usage.PositionalArguments.Where(argument => argument.AssociatedOptionSwitch is null)],
        };

        var resolved = UsageSynopsisParser.ResolveOptionUsage(normalized, options);

        await Assert.That(resolved.PositionalArguments.Select(argument => argument.PropertyName)).IsEquivalentTo(["X", "Y"]);
        await Assert.That(resolved.PositionalArguments.All(argument => argument.AssociatedOptionSwitch is null)).IsTrue();
    }

    [Test]
    [Arguments("[--inherited]...")]
    [Arguments("[--inherited]…")]
    [Arguments("[--inherited]:")]
    [Arguments("[--inherited];")]
    [Arguments("[--inherited],")]
    [Arguments("[--inherited]...:")]
    [Arguments("[--inherited]…,")]
    public async Task Closed_Flag_Groups_Do_Not_Own_Following_Operands(string flag)
    {
        var usage = UsageSynopsisParser.Parse($"Usage: tool run {flag} <TARGET>", ["tool", "run"]);
        var target = usage.PositionalArguments.Single();

        await Assert.That(target.AssociatedOptionSwitch).IsNull();
        await Assert.That(UsageSynopsisParser.IsPositionalSlot(target, [])).IsTrue();
        await Assert.That(target.IsRequired).IsTrue();
    }

    [Test]
    public async Task Resolving_Normalized_Flag_Operands_Preserves_Requiredness_And_Is_Idempotent()
    {
        var usage = UsageSynopsisParser.Parse("Usage: tool run [OPTIONS] --verbose <A> <B>\n       tool run [OPTIONS] <X> <Y>", ["tool", "run"]);
        CliOptionDefinition[] options = [new() { SwitchName = "--verbose", PropertyName = "Verbose", CSharpType = "bool?", IsFlag = true }];
        var normalized = usage with
        {
            PositionalArguments = [.. usage.PositionalArguments.Select(argument => argument with { AssociatedOptionSwitch = null })],
        };

        var resolved = UsageSynopsisParser.ResolveOptionUsage(normalized, options);
        var repeated = UsageSynopsisParser.ResolveOptionUsage(resolved, options);

        await Assert.That(resolved.PositionalArguments.All(argument => argument.IsRequired)).IsTrue();
        await Assert.That(resolved.PositionalArguments.All(argument => argument.CSharpType == "string")).IsTrue();
        await Assert.That(repeated.PositionalArguments).IsEquivalentTo(resolved.PositionalArguments);
    }

    [Test]
    public async Task Ignores_Azure_Usage_Examples()
    {
        const string helpText = """
            Command
                az redis force-reboot : Reboot specified Redis node(s).
                    Usage example - az redis force-reboot --name testCacheName --resource-group
                    testResourceGroup --reboot-type {AllNodes, PrimaryNode, SecondaryNode} [--shard-id].
            """;

        var result = UsageSynopsisParser.Parse(
            helpText,
            ["az", "redis", "force-reboot"]);

        using (Assert.Multiple())
        {
            await Assert.That(result.HasExtractedSynopses).IsFalse();
            await Assert.That(result.HasOperandTokens).IsFalse();
            await Assert.That(result.PositionalArguments).IsEmpty();
        }
    }

    [Test]
    public async Task Parses_Indented_Opted_In_Synopsis_Sections()
    {
        const string helpText = """
            SYNOPSIS
                   aws s3 future-command <source> [destination]
            """;

        var result = UsageSynopsisParser.Parse(
            helpText,
            ["aws", "s3", "future-command"],
            acceptedHeadings: ["usage", "synopsis"]);

        using (Assert.Multiple())
        {
            await Assert.That(result.PositionalArguments.Select(argument => argument.PropertyName))
                .IsEquivalentTo(["Source", "Destination"]);
            await Assert.That(result.PositionalArguments[0].IsRequired).IsTrue();
            await Assert.That(result.PositionalArguments[1].IsRequired).IsFalse();
        }
    }

    [Test]
    public async Task Default_Scraper_Ignores_Synopsis_Sections()
    {
        const string helpText = """
            SYNOPSIS
                tool <TARGET>
            """;

        var result = new CountingUsageScraper().ParseUsage(["tool"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(result.HasExtractedSynopses).IsFalse();
            await Assert.That(result.PositionalArguments).IsEmpty();
        }
    }

    [Test]
    public async Task Ignores_Comma_Separated_Command_Alias()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: brew update, up [options]",
            ["brew", "update"]);
        var resultWithOperand = UsageSynopsisParser.Parse(
            "Usage: tool remove, rm <TARGET>",
            ["tool", "remove"]);
        var resultWithMultipleAliases = UsageSynopsisParser.Parse(
            "Usage: brew uninstall, remove, rm [options] formula|cask [...]",
            ["brew", "uninstall"]);

        using (Assert.Multiple())
        {
            await Assert.That(result.PositionalArguments).IsEmpty();
            await Assert.That(result.HasOperandTokens).IsFalse();
            await Assert.That(resultWithOperand.PositionalArguments).HasSingleItem();
            await Assert.That(resultWithOperand.PositionalArguments.Single().PropertyName)
                .IsEqualTo("Target");
            await Assert.That(resultWithMultipleAliases.PositionalArguments).HasSingleItem();
            await Assert.That(resultWithMultipleAliases.PositionalArguments.Single().PropertyName)
                .IsEqualTo("Formula");
            await Assert.That(resultWithMultipleAliases.PositionalArguments.Single().IsVariadic)
                .IsTrue();
        }
    }

    [Test]
    public async Task Matches_Command_In_Wrapped_Alternative()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: brew bundle [install|upgrade]:",
            ["brew", "bundle", "install"]);

        using (Assert.Multiple())
        {
            await Assert.That(result.CommandMatched).IsTrue();
            await Assert.That(result.MatchedCommandPartCount).IsEqualTo(3);
            await Assert.That(result.HasOperandTokens).IsFalse();
        }
    }

    [Test]
    public async Task Retains_Compound_Endpoint_Placeholders_As_Single_Operands()
    {
        var copy = UsageSynopsisParser.Parse(
            "Usage: minikube cp <source node name>:<source file path> <target node name>:<target absolute file path>",
            ["minikube", "cp"]);
        var mount = UsageSynopsisParser.Parse(
            "Usage: minikube mount <source directory>:<target directory> [flags]",
            ["minikube", "mount"]);

        using (Assert.Multiple())
        {
            await Assert.That(copy.PositionalArguments).Count().IsEqualTo(2);
            await Assert.That(copy.PositionalArguments[0].PropertyName)
                .IsEqualTo("SourceNodeNameSourceFilePath");
            await Assert.That(copy.PositionalArguments[1].PropertyName)
                .IsEqualTo("TargetNodeNameTargetAbsoluteFilePath");
            await Assert.That(mount.PositionalArguments).Count().IsEqualTo(1);
            await Assert.That(mount.PositionalArguments[0].PropertyName)
                .IsEqualTo("SourceDirectoryTargetDirectory");
        }
    }

    [Test]
    public async Task Preserves_Placeholder_Notation_In_Operand_Documentation()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: choco uninstall <pkg> [<pkg2> <pkgN>] packages.config output=<path>",
            ["choco", "uninstall"]);

        using (Assert.Multiple())
        {
            await Assert.That(result.PositionalArguments.Select(argument => argument.Description!))
                .IsEquivalentTo(
                [
                    "The <pkg> operand.",
                    "The <pkg2> <pkgN> operand.",
                    "The packages.config operand.",
                    "The output=<path> operand.",
                ]);
            await Assert.That(GeneratorUtils.EscapeXmlComment(result.PositionalArguments[0].Description))
                .IsEqualTo("The &lt;pkg&gt; operand.");
            await Assert.That(GeneratorUtils.EscapeXmlComment(result.PositionalArguments[1].Description))
                .IsEqualTo("The &lt;pkg2&gt; &lt;pkgN&gt; operand.");
            await Assert.That(GeneratorUtils.EscapeXmlComment(result.PositionalArguments[2].Description))
                .IsEqualTo("The packages.config operand.");
            await Assert.That(GeneratorUtils.EscapeXmlComment(result.PositionalArguments[3].Description))
                .IsEqualTo("The output=&lt;path&gt; operand.");
        }
    }

    [Test]
    public async Task Documents_Only_The_Canonical_Placeholder()
    {
        var alternatives = UsageSynopsisParser.Parse(
            "Usage: newman run [options] <collection|URL>",
            ["newman", "run"]);
        var optionalQualifiers = UsageSynopsisParser.Parse(
            "Usage: pulumi env get [<org-name>/][<project-name>/]<environment-name>[@<version>] [property-path]",
            ["pulumi", "env", "get"]);
        var compound = UsageSynopsisParser.Parse(
            "Usage: pulumi env clone [<org-name>/]<src-project-name>/<src-environment-name> [<dest-project-name>/]<dest-environment-name>",
            ["pulumi", "env", "clone"]);

        using (Assert.Multiple())
        {
            await Assert.That(alternatives.PositionalArguments[0].Description)
                .IsEqualTo("The collection operand.");
            await Assert.That(optionalQualifiers.PositionalArguments[0].Description)
                .IsEqualTo("The <environment-name> operand.");
            await Assert.That(compound.PositionalArguments[0].Description)
                .IsEqualTo("The <src-environment-name> operand.");
        }
    }

    [Test]
    public async Task Preserves_Variadic_Placeholder_In_Operand_Documentation()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool upload <FILES...>",
            ["tool", "upload"]);

        await Assert.That(result.PositionalArguments.Single().Description)
            .IsEqualTo("The <FILES...> operand.");
    }

    [Test]
    public async Task Command_Group_Placeholders_Remain_Executable_Operands()
    {
        var parsed = UsageSynopsisParser.Parse(
            "Usage: docker buildx [OPTIONS] COMMAND",
            ["docker", "buildx"]);
        var nested = UsageSynopsisParser.Parse(
            "Usage: winget source [<command>] [<options>]",
            ["winget", "source"]);

        using (Assert.Multiple())
        {
            await Assert.That(parsed.PositionalArguments.Single().PropertyName)
                .IsEqualTo("Command");
            await Assert.That(parsed.HasOperandTokens).IsTrue();
            await Assert.That(nested.PositionalArguments.Single().PropertyName)
                .IsEqualTo("Command");
            await Assert.That(nested.HasOperandTokens).IsTrue();
        }
    }

    [Test]
    public async Task Parses_Regression_Operand_Syntax_Through_One_Model()
    {
        var fixtures = new[]
        {
            Fixture(
                "vault",
                "Usage: vault delete [options] PATH",
                ["vault", "delete"],
                Required("Path", phase: CommandLinePhase.Passthrough)),
            Fixture(
                "terraform",
                "Usage: terraform [global options] import [options] ADDRESS ID",
                ["terraform", "import"],
                Required("Address", phase: CommandLinePhase.Passthrough),
                Required("Id", phase: CommandLinePhase.Passthrough)),
            Fixture(
                "cargo",
                "Usage: cargo run [OPTIONS] [ARGS]...",
                ["cargo", "run"],
                Optional("Args", isVariadic: true, phase: CommandLinePhase.Passthrough)),
            Fixture(
                "packer",
                "Usage: packer build [options] TEMPLATE",
                ["packer", "build"],
                Required("Template", phase: CommandLinePhase.Passthrough)),
            Fixture(
                "gh",
                "USAGE\n  gh api <endpoint> [flags]",
                ["gh", "api"],
                Required("Endpoint")),
            Fixture(
                "podman",
                "Usage:\n  podman attach [options] CONTAINER",
                ["podman", "attach"],
                Required("Container", phase: CommandLinePhase.Passthrough)),
            Fixture(
                "buildah",
                "Usage: buildah add [options] container source [source ...] destination",
                ["buildah", "add"],
                Required("Container", phase: CommandLinePhase.Passthrough),
                Required("Source", isVariadic: true, phase: CommandLinePhase.Passthrough),
                Required("Destination", phase: CommandLinePhase.Passthrough)),
            Fixture(
                "newman",
                "Usage: newman run [options] <collection|URL>",
                ["newman", "run"],
                Required("Collection", phase: CommandLinePhase.Passthrough)),
            Fixture(
                "docker",
                "Usage: docker exec [OPTIONS] CONTAINER COMMAND [ARG...]",
                ["docker", "exec"],
                Required("Container", phase: CommandLinePhase.Passthrough),
                Required("Command", phase: CommandLinePhase.Passthrough),
                Optional("Arg", isVariadic: true, phase: CommandLinePhase.Passthrough)),
        };

        foreach (var fixture in fixtures)
        {
            var result = UsageSynopsisParser.Parse(fixture.HelpText, fixture.CommandPath);

            await Assert.That(result.HasOperandTokens)
                .IsTrue()
                .Because(fixture.Tool);
            await Assert.That(result.PositionalArguments.Count)
                .IsEqualTo(fixture.ExpectedArguments.Count)
                .Because(fixture.Tool);

            for (var index = 0; index < fixture.ExpectedArguments.Count; index++)
            {
                var actual = result.PositionalArguments[index];
                var expected = fixture.ExpectedArguments[index];
                await Assert.That(actual.PropertyName).IsEqualTo(expected.PropertyName).Because(fixture.Tool);
                await Assert.That(actual.IsRequired).IsEqualTo(expected.IsRequired).Because(fixture.Tool);
                await Assert.That(actual.IsVariadic).IsEqualTo(expected.IsVariadic).Because(fixture.Tool);
                await Assert.That(actual.Phase).IsEqualTo(expected.Phase).Because(fixture.Tool);
                await Assert.That(actual.PositionIndex).IsEqualTo(index).Because(fixture.Tool);
            }
        }
    }

    [Test]
    public async Task Preserves_Operand_Order_Around_Options()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool copy <SOURCE> [OPTIONS] <DESTINATION>",
            ["tool", "copy"]);

        var source = result.PositionalArguments.Single(argument => argument.PropertyName == "Source");
        var destination = result.PositionalArguments.Single(argument => argument.PropertyName == "Destination");

        await Assert.That(source.Phase).IsEqualTo(CommandLinePhase.EarlyOperand);
        await Assert.That(source.PositionIndex).IsEqualTo(0);
        await Assert.That(destination.Phase).IsEqualTo(CommandLinePhase.Passthrough);
        await Assert.That(destination.PositionIndex).IsEqualTo(0);

        var globalOptions = UsageSynopsisParser.Parse(
            "Usage: tool [GLOBAL OPTIONS] copy <SOURCE>",
            ["tool", "copy"]);

        await Assert.That(globalOptions.PositionalArguments.Single().Phase)
            .IsEqualTo(CommandLinePhase.Passthrough);
    }

    [Test]
    public async Task Parses_Required_Optional_And_Repeat_Markers()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool deploy TARGET [environment] [FILE...]",
            ["tool", "deploy"]);

        await Assert.That(result.PositionalArguments.Count).IsEqualTo(3);
        await Assert.That(result.PositionalArguments[0].CSharpType).IsEqualTo("string");
        await Assert.That(result.PositionalArguments[1].CSharpType).IsEqualTo("string?");
        await Assert.That(result.PositionalArguments[2].CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(result.PositionalArguments[2].IsVariadic).IsTrue();
    }

    [Test]
    [Arguments("[--no-secrets]:")]
    [Arguments("[--json]:")]
    [Arguments("[--debug]:")]
    [Arguments("[--file=]:")]
    public async Task Ignores_Punctuated_Option_Tokens(string optionToken)
    {
        var result = UsageSynopsisParser.Parse(
            $"Usage: tool run [target] {optionToken}",
            ["tool", "run"]);

        var argument = result.PositionalArguments.Single();
        await Assert.That(argument.PropertyName).IsEqualTo("Target");
    }

    [Test]
    public async Task Parses_Forwarded_Option_Tail_As_Multiple_Arguments()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: docker top CONTAINER [ps OPTIONS]",
            ["docker", "top"]);

        var psOptions = result.PositionalArguments.Single(argument =>
            argument.PropertyName == "PsOptions");
        using (Assert.Multiple())
        {
            await Assert.That(psOptions.IsRequired).IsFalse();
            await Assert.That(psOptions.IsVariadic).IsTrue();
            await Assert.That(psOptions.CSharpType).IsEqualTo("IEnumerable<string>?");
        }
    }

    [Test]
    [Arguments("")]
    [Arguments(":")]
    public async Task Splits_Nested_Command_And_Argument_Operands(string trailingPunctuation)
    {
        var result = UsageSynopsisParser.Parse(
            $"Usage: podman run [options] IMAGE [COMMAND [ARG...]]{trailingPunctuation}",
            ["podman", "run"]);

        using (Assert.Multiple())
        {
            await Assert.That(result.PositionalArguments).Count().IsEqualTo(3);
            await Assert.That(result.PositionalArguments[0].PropertyName).IsEqualTo("Image");
            await Assert.That(result.PositionalArguments[0].IsRequired).IsTrue();
            await Assert.That(result.PositionalArguments[1].PropertyName).IsEqualTo("Command");
            await Assert.That(result.PositionalArguments[1].CSharpType).IsEqualTo("string?");
            await Assert.That(result.PositionalArguments[2].PropertyName).IsEqualTo("Arg");
            await Assert.That(result.PositionalArguments[2].CSharpType)
                .IsEqualTo("IEnumerable<string>?");
        }
    }

    [Test]
    public async Task Requires_Suffixes_Outside_Optional_Qualifiers()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: podman cp [options] [CONTAINER:]SRC_PATH [CONTAINER:]DEST_PATH",
            ["podman", "cp"]);

        await Assert.That(result.PositionalArguments).Count().IsEqualTo(2);
        await Assert.That(result.PositionalArguments.All(argument => argument.IsRequired)).IsTrue();
        await Assert.That(result.PositionalArguments.All(argument => argument.CSharpType == "string"))
            .IsTrue();
    }

    [Test]
    public async Task Preserves_Variadic_Marker_After_Alternative()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: docker inspect [OPTIONS] NAME|ID [NAME|ID...]",
            ["docker", "inspect"]);

        var argument = result.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(argument.PropertyName).IsEqualTo("Name");
            await Assert.That(argument.IsRequired).IsTrue();
            await Assert.That(argument.IsVariadic).IsTrue();
            await Assert.That(argument.CSharpType).IsEqualTo("IEnumerable<string>");
        }
    }

    [Test]
    [Arguments("Usage: brew services stop (formula | --all)")]
    [Arguments("Usage: brew services stop (--all | formula)")]
    [Arguments("Usage: brew services stop (<formula>|--all):")]
    [Arguments("Usage: [sudo] brew services stop [--keep] [--no-wait|--max-wait=] (<formula>|--all):")]
    public async Task Models_Operand_Or_Option_Alternatives_As_Optional(string helpText)
    {
        var result = UsageSynopsisParser.Parse(
            helpText,
            ["brew", "services", "stop"]);

        var argument = result.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(argument.PropertyName).IsEqualTo("Formula");
            await Assert.That(argument.IsRequired).IsFalse();
            await Assert.That(argument.CSharpType).IsEqualTo("string?");
        }
    }

    [Test]
    public async Task Joins_Wrapped_Operand_With_Trailing_Colon()
    {
        const string helpText = """
            Usage: brew services stop [--keep] [--no-wait|--max-wait=]
            (<formula>|--all):
            """;

        var result = UsageSynopsisParser.Parse(
            helpText,
            ["brew", "services", "stop"]);

        var argument = result.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(argument.PropertyName).IsEqualTo("Formula");
            await Assert.That(argument.IsRequired).IsFalse();
            await Assert.That(argument.CSharpType).IsEqualTo("string?");
        }
    }

    [Test]
    [Arguments("SRC_PATH|-")]
    [Arguments("-|SRC_PATH")]
    public async Task Treats_Lone_Dash_Alternative_As_A_Required_Operand(string sourceAlternative)
    {
        var result = UsageSynopsisParser.Parse(
            $"Usage: docker cp {sourceAlternative} CONTAINER:DEST_PATH",
            ["docker", "cp"]);

        var source = result.PositionalArguments[0];
        using (Assert.Multiple())
        {
            await Assert.That(source.PropertyName).IsEqualTo("SrcPath");
            await Assert.That(source.IsRequired).IsTrue();
            await Assert.That(source.CSharpType).IsEqualTo("string");
        }
    }

    [Test]
    [Arguments("--format=<json|yaml>")]
    [Arguments("[--format <json|yaml>]")]
    [Arguments("[--format {json|yaml}]")]
    [Arguments("[--format [json|yaml]]")]
    [Arguments("[--format JSON|YAML]")]
    [Arguments("[--format JSON | YAML]")]
    [Arguments("[-f|--format <json|yaml>]")]
    [Arguments("[-f|--format JSON|YAML]")]
    [Arguments("[-f|--format=<json|yaml>]")]
    [Arguments("[-f|--format=JSON|YAML]")]
    [Arguments("[-f | --format <json|yaml>]")]
    [Arguments("[-f | --format JSON | YAML]")]
    [Arguments("[-f | --format | --output <json|yaml>]")]
    public async Task Does_Not_Model_Option_Value_Alternatives_As_Operands(string option)
    {
        var result = UsageSynopsisParser.Parse(
            $"Usage: tool run {option}",
            ["tool", "run"]);

        await Assert.That(result.PositionalArguments).IsEmpty();
    }

    [Test]
    [Arguments("--config|KEY=VALUE")]
    [Arguments("KEY=VALUE|--config")]
    public async Task Models_Assignment_Operand_Alternatives_As_Optional(string alternative)
    {
        var result = UsageSynopsisParser.Parse(
            $"Usage: tool run {alternative}",
            ["tool", "run"]);

        var argument = result.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(argument.PropertyName).IsEqualTo("KeyValue");
            await Assert.That(argument.IsRequired).IsFalse();
            await Assert.That(argument.CSharpType).IsEqualTo("string?");
        }
    }

    [Test]
    public async Task Spaced_Mixed_Alternatives_Are_Not_Option_Values()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool run (--config | KEY=VALUE | FILE)",
            ["tool", "run"]);

        var argument = result.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(argument.PropertyName).IsEqualTo("KeyValue");
            await Assert.That(argument.IsRequired).IsFalse();
        }
    }

    [Test]
    public async Task Does_Not_Model_Option_Control_Alternatives_As_Operands()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool run (options | --all)",
            ["tool", "run"]);

        await Assert.That(result.PositionalArguments).IsEmpty();
    }

    [Test]
    public async Task Precommand_Mixed_Alternatives_Select_Passthrough_Phase()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool (--config | CONFIG) run <SOURCE>",
            ["tool", "run"]);

        var source = result.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(source.PropertyName).IsEqualTo("Source");
            await Assert.That(source.Phase).IsEqualTo(CommandLinePhase.Passthrough);
        }
    }

    [Test]
    [Arguments("--fast|DEST")]
    [Arguments("DEST|--fast")]
    public async Task Mixed_Alternatives_Transition_Following_Operands(string alternative)
    {
        var result = UsageSynopsisParser.Parse(
            $"Usage: tool copy SOURCE ({alternative}) TARGET",
            ["tool", "copy"]);

        using (Assert.Multiple())
        {
            await Assert.That(result.PositionalArguments[0].PropertyName).IsEqualTo("Source");
            await Assert.That(result.PositionalArguments[0].Phase).IsEqualTo(CommandLinePhase.EarlyOperand);
            await Assert.That(result.PositionalArguments[1].PropertyName).IsEqualTo("Dest");
            await Assert.That(result.PositionalArguments[1].IsRequired).IsFalse();
            await Assert.That(result.PositionalArguments[1].Phase).IsEqualTo(CommandLinePhase.EarlyOperand);
            await Assert.That(result.PositionalArguments[2].PropertyName).IsEqualTo("Target");
            await Assert.That(result.PositionalArguments[2].Phase).IsEqualTo(CommandLinePhase.Passthrough);
        }
    }

    [Test]
    public async Task Applies_Bracketed_Standalone_Repeat_To_Preceding_Operand()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: podman inspect [options] {CONTAINER|IMAGE} [...]",
            ["podman", "inspect"]);

        var argument = result.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(argument.IsRequired).IsTrue();
            await Assert.That(argument.IsVariadic).IsTrue();
            await Assert.That(argument.CSharpType).IsEqualTo("IEnumerable<string>");
        }
    }

    [Test]
    public async Task Preserves_Required_Core_Inside_Optional_Operand_Qualifiers()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: pulumi env get [<org-name>/][<project-name>/]<environment-name>[@<version>] [property-path]",
            ["pulumi", "env", "get"]);

        using (Assert.Multiple())
        {
            await Assert.That(result.PositionalArguments).Count().IsEqualTo(2);
            await Assert.That(result.PositionalArguments[0].PropertyName).IsEqualTo("EnvironmentName");
            await Assert.That(result.PositionalArguments[0].IsRequired).IsTrue();
            await Assert.That(result.PositionalArguments[0].CSharpType).IsEqualTo("string");
            await Assert.That(result.PositionalArguments[1].PropertyName).IsEqualTo("PropertyPath");
            await Assert.That(result.PositionalArguments[1].IsRequired).IsFalse();
            await Assert.That(result.PositionalArguments[1].CSharpType).IsEqualTo("string?");
        }
    }

    [Test]
    public async Task Selects_Final_Required_Placeholder_From_Qualified_Compound_Operand()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: pulumi env clone [<org-name>/]<src-project-name>/<src-environment-name> [<dest-project-name>/]<dest-environment-name>",
            ["pulumi", "env", "clone"]);

        using (Assert.Multiple())
        {
            await Assert.That(result.PositionalArguments).Count().IsEqualTo(2);
            await Assert.That(result.PositionalArguments[0].PropertyName).IsEqualTo("SrcEnvironmentName");
            await Assert.That(result.PositionalArguments[0].IsRequired).IsTrue();
            await Assert.That(result.PositionalArguments[1].PropertyName).IsEqualTo("DestEnvironmentName");
            await Assert.That(result.PositionalArguments[1].IsRequired).IsTrue();
        }
    }

    [Test]
    [Arguments("client-secret", "ClientSecret")]
    [Arguments("secret-access-key", "SecretAccessKey")]
    [Arguments("access-token", "AccessToken")]
    public async Task Marks_Secret_Positional_Operands(string operandName, string propertyName)
    {
        var result = UsageSynopsisParser.Parse(
            $"Usage: tool login <{operandName}>",
            ["tool", "login"]);

        var argument = result.PositionalArguments.Single();

        await Assert.That(argument.PropertyName).IsEqualTo(propertyName);
        await Assert.That(argument.IsSecret).IsTrue();
    }

    [Test]
    public async Task Preserves_Operands_Grouped_Behind_Option_Terminator()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: cargo login [OPTIONS] [-- [args]...]",
            ["cargo", "login"]);

        var argument = result.PositionalArguments.Single();

        await Assert.That(result.HasOperandTokens).IsTrue();
        await Assert.That(argument.PropertyName).IsEqualTo("Args");
        await Assert.That(argument.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(argument.IsVariadic).IsTrue();
        await Assert.That(argument.Phase).IsEqualTo(CommandLinePhase.Passthrough);
        await Assert.That(argument.PrependOptionTerminator).IsTrue();
    }

    [Test]
    public async Task Places_Operands_After_Terminated_Passthrough_In_Late_Phase()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: gh codespace cp [-e] [-r] [-- [<scp flags>...]] <sources>... <dest>",
            ["gh", "codespace", "cp"]);

        var arguments = result.PositionalArguments;

        await Assert.That(arguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["ScpFlags", "Sources", "Dest"]);
        await Assert.That(arguments[0].CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(arguments[0].Phase).IsEqualTo(CommandLinePhase.Passthrough);
        await Assert.That(arguments[0].PrependOptionTerminator).IsTrue();
        await Assert.That(arguments[0].AssociatedOptionSwitch).IsNull();
        await Assert.That(arguments.Skip(1).All(argument =>
                argument.Phase == CommandLinePhase.LateOperand))
            .IsTrue();
    }

    [Test]
    public async Task Parses_Inline_Usage_Continuation_Lines()
    {
        const string helpText = """
            Usage: tool upload [OPTIONS]
                   <SOURCE> <DESTINATION>

            Options:
              --force
            """;

        var result = UsageSynopsisParser.Parse(helpText, ["tool", "upload"]);

        await Assert.That(result.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["Source", "Destination"]);
        await Assert.That(result.PositionalArguments.All(argument =>
                argument.Phase == CommandLinePhase.Passthrough))
            .IsTrue();
    }

    [Test]
    public async Task Ignores_Explanation_After_Terminated_Wrapped_Operand()
    {
        const string helpText = """
            Usage:
              minikube profile [MINIKUBE_PROFILE_NAME]. You can return to the default profile [flags]
            """;

        var result = UsageSynopsisParser.Parse(helpText, ["minikube", "profile"]);

        var argument = result.PositionalArguments.Single();
        await Assert.That(argument.PropertyName).IsEqualTo("MinikubeProfileName");
        await Assert.That(argument.IsRequired).IsFalse();
    }

    [Test]
    public async Task Carries_Standalone_Option_Terminator_To_Following_Operand()
    {
        var required = UsageSynopsisParser.Parse(
            "Usage: tool run [OPTIONS] -- <ARGS>",
            ["tool", "run"]);
        var optional = UsageSynopsisParser.Parse(
            "Usage: tool run [OPTIONS] [--] [ARGS...]",
            ["tool", "run"]);

        await Assert.That(required.PositionalArguments.Single().PrependOptionTerminator).IsTrue();
        await Assert.That(optional.PositionalArguments.Single().PrependOptionTerminator).IsTrue();
    }

    [Test]
    public async Task Standalone_Option_Terminator_Clears_Prior_Option_Association()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool run [--verbose] -- <args>",
            ["tool", "run"]);

        var argument = result.PositionalArguments.Single();
        await Assert.That(argument.PrependOptionTerminator).IsTrue();
        await Assert.That(argument.AssociatedOptionSwitch).IsNull();
    }

    [Test]
    public async Task Nested_Operand_Group_Preserves_Option_Terminator()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool run [-- [<left>] [<right>]]",
            ["tool", "run"]);

        await Assert.That(result.PositionalArguments).Count().IsEqualTo(2);
        await Assert.That(result.PositionalArguments[0].PrependOptionTerminator).IsTrue();
        await Assert.That(result.PositionalArguments[1].PrependOptionTerminator).IsTrue();
    }

    [Test]
    public async Task Does_Not_Match_Selected_Option_Operand_To_Alternative_Positional()
    {
        const string helpText = """
            Usage:
              tool run [OPTIONS] --file <FILE> <TARGET>
              tool run [OPTIONS] <OBJECT>
            """;

        var result = UsageSynopsisParser.Parse(helpText, ["tool", "run"]);
        var file = result.PositionalArguments.Single(argument => argument.PropertyName == "File");

        using (Assert.Multiple())
        {
            await Assert.That(file.AssociatedOptionSwitch).IsEqualTo("--file");
            await Assert.That(file.IsRequired).IsFalse();
            await Assert.That(file.CSharpType).IsEqualTo("string?");
        }
    }

    [Test]
    [Arguments("<A> --file <FILE> <B>", "<X> <Y>", "B")]
    [Arguments("--file <FILE> <TARGET>", "<OBJECT> --mode <MODE>", "Target")]
    [Arguments("--file <FILE> <TARGET>", "<OBJECT>", "Target")]
    public async Task Matches_Renamed_Positionals_After_Filtering_Option_Operands(
        string selectedForm,
        string alternativeForm,
        string propertyName)
    {
        var helpText = $"Usage:\n  tool run [OPTIONS] {selectedForm}\n  tool run [OPTIONS] {alternativeForm}";

        var result = UsageSynopsisParser.Parse(helpText, ["tool", "run"]);
        var positional = result.PositionalArguments.Single(argument => argument.PropertyName == propertyName);

        using (Assert.Multiple())
        {
            await Assert.That(positional.AssociatedOptionSwitch).IsNull();
            await Assert.That(positional.IsRequired).IsTrue();
            await Assert.That(positional.CSharpType).IsEqualTo("string");
        }
    }

    [Test]
    public async Task Relaxes_Operands_Absent_From_Alternate_Invocation_Forms()
    {
        const string helpText = """
            Usage:
              cargo add <DEP>[@<VERSION>]
              cargo add --path <PATH>
              cargo add --git <URL>
            """;

        var result = UsageSynopsisParser.Parse(helpText, ["cargo", "add"]);
        var dependency = result.PositionalArguments.Single();
        var requiredSources = result.RequiredAlternativeGroups.Single().Members
            .Select(member => (member.OptionSwitch ?? member.PositionalPropertyName)!);

        using (Assert.Multiple())
        {
            await Assert.That(result.MatchedSynopsisCount).IsEqualTo(3);
            await Assert.That(dependency.IsRequired).IsFalse();
            await Assert.That(dependency.CSharpType).IsEqualTo("string?");
            await Assert.That(requiredSources).IsEquivalentTo(["Dep", "--path", "--git"]);
        }
    }

    [Test]
    public async Task Relaxes_Operands_Absent_From_Alternate_Forms_With_Option_Placeholders_And_Ellipses()
    {
        // cargo 1.98 prints the three forms with [OPTIONS] and a trailing " ..." each.
        const string helpText = """
            Usage: cargo add [OPTIONS] <DEP>[@<VERSION>] ...
                   cargo add [OPTIONS] --path <PATH> ...
                   cargo add [OPTIONS] --git <URL> ...
            """;

        var result = UsageSynopsisParser.Parse(helpText, ["cargo", "add"]);
        var dependency = result.PositionalArguments.Single();
        var requiredSources = result.RequiredAlternativeGroups.Single().Members
            .Select(member => (member.OptionSwitch ?? member.PositionalPropertyName)!);

        using (Assert.Multiple())
        {
            await Assert.That(result.MatchedSynopsisCount).IsEqualTo(3);
            await Assert.That(dependency.IsRequired).IsFalse();
            await Assert.That(dependency.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(requiredSources).IsEquivalentTo(["Dep", "--path", "--git"]);
        }
    }

    [Test]
    public async Task Keeps_Required_Operand_Positions_Across_Renamed_Alternate_Forms()
    {
        const string helpText = """
            Usage:
              podman compose cp [OPTIONS] SERVICE:SRC_PATH DEST_PATH|-
              podman compose cp [OPTIONS] SRC_PATH|- SERVICE:DEST_PATH
            """;

        var result = UsageSynopsisParser.Parse(
            helpText,
            ["podman", "compose", "cp"]);

        using (Assert.Multiple())
        {
            await Assert.That(result.PositionalArguments).Count().IsEqualTo(2);
            await Assert.That(result.PositionalArguments.All(argument => argument.IsRequired)).IsTrue();
            await Assert.That(result.PositionalArguments.All(argument => argument.CSharpType == "string")).IsTrue();
        }
    }

    [Test]
    public async Task Models_Standalone_Switch_On_Separate_Synopsis_As_Required_Alternative()
    {
        const string helpText = """
            Usage:
              tool clean <TARGET>
              tool clean --all
            """;

        var result = UsageSynopsisParser.Parse(helpText, ["tool", "clean"]);
        var requiredSources = result.RequiredAlternativeGroups.Single().Members
            .Select(member => (member.OptionSwitch ?? member.PositionalPropertyName)!);

        await Assert.That(requiredSources).IsEquivalentTo(["Target", "--all"]);
    }

    [Test]
    public async Task Collapses_Option_Aliases_Before_Counting_Alternative_Branches()
    {
        const string helpText = """
            Usage:
              tool clean <TARGET>
              tool clean (-a|--all)
            """;

        var result = UsageSynopsisParser.Parse(helpText, ["tool", "clean"]);
        var requiredSources = result.RequiredAlternativeGroups.Single().Members
            .Select(member => (member.OptionSwitch ?? member.PositionalPropertyName)!);

        await Assert.That(requiredSources).IsEquivalentTo(["Target", "--all"]);
    }

    [Test]
    public async Task Does_Not_Flatten_Conjunctive_Alternative_Branches()
    {
        const string helpText = """
            Usage:
              tool copy <SOURCE> <DESTINATION>
              tool copy --left <INPUT> --right <OUTPUT>
            """;

        var result = UsageSynopsisParser.Parse(helpText, ["tool", "copy"]);

        var group = result.RequiredAlternativeGroups.Single();
        await Assert.That(group.Members).IsEmpty();
        await Assert.That(group.Groups.Count).IsEqualTo(2);
        await Assert.That(group.Groups.All(branch => !branch.IsChoice && branch.Members.Count == 2)).IsTrue();
    }

    [Test]
    public async Task Does_Not_Flatten_Parenthesized_Conjunctive_Alternative_Branches()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool clean (<TARGET>|(--force --all))",
            ["tool", "clean"]);

        await Assert.That(result.RequiredAlternativeGroups).IsEmpty();
    }

    [Test]
    public async Task Does_Not_Treat_Conjunctive_Short_And_Long_Switches_As_Aliases()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool clean (<TARGET>|(-f --all))",
            ["tool", "clean"]);

        await Assert.That(result.RequiredAlternativeGroups).IsEmpty();
    }

    [Test]
    public async Task Collapses_Inline_Option_Aliases_Before_Modeling_Alternatives()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool clean (<TARGET>|(-a|--all))",
            ["tool", "clean"]);
        var members = result.RequiredAlternativeGroups.Single().Members
            .Select(member => (member.OptionSwitch ?? member.PositionalPropertyName)!);

        using (Assert.Multiple())
        {
            await Assert.That(members).IsEquivalentTo(["Target", "--all"]);
            await Assert.That(result.PositionalArguments.Single().IsRequired).IsFalse();
        }
    }

    [Test]
    public async Task Collapses_Comma_Separated_Option_Aliases()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool clean (<TARGET>|-a, --all)",
            ["tool", "clean"]);
        var members = result.RequiredAlternativeGroups.Single().Members
            .Select(member => (member.OptionSwitch ?? member.PositionalPropertyName)!);

        await Assert.That(members).IsEquivalentTo(["Target", "--all"]);
    }

    [Test]
    public async Task Preserves_Case_Distinct_Option_Alternatives()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool save (<TARGET>|-o|-O)",
            ["tool", "save"]);
        var members = result.RequiredAlternativeGroups.Single().Members
            .Select(member => (member.OptionSwitch ?? member.PositionalPropertyName)!);

        await Assert.That(members).IsEquivalentTo(["Target", "-o", "-O"]);
    }

    [Test]
    public async Task Attached_Value_In_Nested_Group_Does_Not_Own_Following_Operand()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool run <[--format=VALUE] file>",
            ["tool", "run"]);

        await Assert.That(result.PositionalArguments.Single().AssociatedOptionSwitch).IsNull();
    }

    [Test]
    public async Task Does_Not_Infer_Optional_Switches_As_Required_Alternatives()
    {
        const string helpText = """
            Usage:
              tool run [--json] <FILE>
              tool run [--yaml] <FILE>
            """;

        var result = UsageSynopsisParser.Parse(helpText, ["tool", "run"]);

        await Assert.That(result.RequiredAlternativeGroups).IsEmpty();
    }

    [Test]
    public async Task Models_Required_Inline_Option_Operand_Alternatives()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: cargo add [OPTIONS] <DEP_ID|--path <PATH>|--git <URI>>...",
            ["cargo", "add"]);
        var dependency = result.PositionalArguments.Single();
        var requiredSources = result.RequiredAlternativeGroups.Single().Members
            .Select(member => (member.OptionSwitch ?? member.PositionalPropertyName)!);

        using (Assert.Multiple())
        {
            await Assert.That(dependency.PropertyName).IsEqualTo("DepId");
            await Assert.That(dependency.IsRequired).IsFalse();
            await Assert.That(requiredSources).IsEquivalentTo(["DepId", "--path", "--git"]);
        }
    }

    [Test]
    public async Task Does_Not_Enforce_Inline_Group_Bypassed_By_Alternate_Synopsis()
    {
        const string helpText = """
            Usage:
              tool import (<FILE>|--url <URL>)
              tool import --stdin
            """;

        var result = UsageSynopsisParser.Parse(helpText, ["tool", "import"]);

        await Assert.That(result.RequiredAlternativeGroups).IsEmpty();
    }

    [Test]
    public async Task Models_Standalone_Flags_As_Required_Alternative_Members()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: brew services stop (<formula>|--all)",
            ["brew", "services", "stop"]);
        var requiredSources = result.RequiredAlternativeGroups.Single().Members
            .Select(member => (member.OptionSwitch ?? member.PositionalPropertyName)!);

        await Assert.That(requiredSources).IsEquivalentTo(["Formula", "--all"]);
    }

    [Test]
    public async Task Models_Attached_Value_Option_As_Required_Alternative_Member()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool send (<FILE>|--data=<TEXT>)",
            ["tool", "send"]);
        var requiredSources = result.RequiredAlternativeGroups.Single().Members
            .Select(member => (member.OptionSwitch ?? member.PositionalPropertyName)!);

        await Assert.That(requiredSources).IsEquivalentTo(["File", "--data"]);
    }

    [Test]
    public async Task Splits_Parenthesized_Option_Aliases_Into_Real_Switches()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: git checkout (-p|--patch)",
            ["git", "checkout"]);

        await Assert.That(result.RequiredOptionSwitches).IsEquivalentTo(["-p", "--patch"]);
        await Assert.That(result.RequiredOptionSwitches).DoesNotContain("-p|--patch");
    }

    [Test]
    public async Task Does_Not_Model_Operand_Aliases_As_Separate_Required_Properties()
    {
        var result = UsageSynopsisParser.Parse(
            "Usage: tool run <FILE|DIRECTORY>",
            ["tool", "run"]);

        await Assert.That(result.RequiredAlternativeGroups).IsEmpty();
        await Assert.That(result.PositionalArguments.Single().PropertyName).IsEqualTo("File");
    }

    [Test]
    public async Task Relaxes_Operands_When_An_Alternate_Form_Is_Operandless()
    {
        const string helpText = """
            Usage:
              tool run <FILE>
              tool run
            """;

        var result = UsageSynopsisParser.Parse(helpText, ["tool", "run"]);
        var file = result.PositionalArguments.Single();

        await Assert.That(file.IsRequired).IsFalse();
        await Assert.That(file.CSharpType).IsEqualTo("string?");
    }

    [Test]
    public async Task Prefers_Full_Command_Path_Over_Suffix_With_More_Operands()
    {
        const string helpText = """
            Usage:
              tool config get <TARGET>
              get <WRONG> <EXTRA>
            """;

        var result = UsageSynopsisParser.Parse(
            helpText,
            ["tool", "config", "get"]);

        await Assert.That(result.Synopsis).IsEqualTo("tool config get <TARGET>");
        await Assert.That(result.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["Target"]);
    }

    [Test]
    public async Task Reports_Equally_Ranked_Synopses_As_Ambiguous()
    {
        const string helpText = """
            Usage:
              tool run <FILE>
              tool run <TARGET>
            """;

        var result = UsageSynopsisParser.Parse(helpText, ["tool", "run"]);

        await Assert.That(result.MatchedSynopsisCount).IsEqualTo(2);
        await Assert.That(result.HasAmbiguousMatch).IsTrue();
    }

    [Test]
    public async Task Shared_Traversal_Parses_Usage_Once()
    {
        var scraper = new CountingUsageScraper();
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(scraper.UsageParseCount).IsEqualTo(1);
        await Assert.That(commands.Single().PositionalArguments.Single().PropertyName)
            .IsEqualTo("Target");
    }

    [Test]
    public async Task Kustomize_Adapter_Supplies_Omitted_Buildmetadata_Operand()
    {
        const string helpText = """
            Adds build metadata.

            Usage:
              kustomize edit add buildmetadata [flags]

            Flags:
              -h, --help   help for buildmetadata
            """;

        var command = await new TestKustomizeCliScraper().Parse(
            ["kustomize", "edit", "add", "buildmetadata"],
            helpText);

        var metadata = command!.PositionalArguments.Single();
        await Assert.That(metadata.PropertyName).IsEqualTo("Metadata");
        await Assert.That(metadata.IsRequired).IsTrue();
    }

    [Test]
    public async Task Migrated_Scraper_Rejects_Missing_Shared_Usage_Result()
    {
        var scraper = new TestKustomizeCliScraper();

        await Assert.That(() => scraper.ParseWithoutUsage(["kustomize", "build"], "Usage: kustomize build"))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("Shared traversal must pass its parsed synopsis");
    }

    [Test]
    public async Task Newman_Does_Not_Promote_Wrapped_Alternate_To_Subcommand()
    {
        const string helpText = """
            Commands:
              run [options] <collection|URL>  Run a collection
                  URL                       Wrapped description continuation
            """;

        var commands = new TestNewmanCliScraper().Extract(helpText);

        await Assert.That(commands).IsEquivalentTo(["run"]);
    }

    [Test]
    public async Task Generator_Renders_Required_Operands_As_Constructor_Parameters_And_Optional_As_Properties()
    {
        var usage = UsageSynopsisParser.Parse(
            "Usage: tool upload <SOURCE> [DESTINATION...]",
            ["tool", "upload"]);
        var command = new CliCommandDefinition
        {
            FullCommand = "tool upload",
            CommandParts = ["upload"],
            ClassName = "ToolUploadOptions",
            ParentClassName = "ToolOptions",
            ToolNamespacePrefix = "Tool",
            Options = [],
            PositionalArguments = usage.PositionalArguments,
            UsageSynopsis = usage.Synopsis,
            HasOperandTakingUsage = usage.HasOperandTokens,
        };
        var tool = new CliToolDefinition
        {
            ToolName = "tool",
            NamespacePrefix = "Tool",
            TargetNamespace = "ModularPipelines.Tool",
            OutputDirectory = "src/ModularPipelines.Tool",
            Commands = [command],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;

        await Assert.That(generated).Contains(
            $"[CliArgument(0, Phase = CommandLinePhase.EarlyOperand, Required = true)]{Environment.NewLine}    public string Source {{ get; private init; }}");
        await Assert.That(generated).Contains("public ToolUploadOptions(");
        await Assert.That(generated).Contains("ArgumentNullException.ThrowIfNull(Source)");
        await Assert.That(generated).Contains(
            "[CliArgument(1, Phase = CommandLinePhase.EarlyOperand)]");
        await Assert.That(generated).Contains(
            "public IEnumerable<string>? Destination { get; set; }");
    }

    [Test]
    public async Task Generator_Emits_Option_Terminator_Metadata()
    {
        var usage = UsageSynopsisParser.Parse(
            "Usage: cargo test [OPTIONS] [-- [ARGS]...]",
            ["cargo", "test"]);
        var command = new CliCommandDefinition
        {
            FullCommand = "cargo test",
            CommandParts = ["test"],
            ClassName = "CargoTestOptions",
            ParentClassName = "CargoOptions",
            ToolNamespacePrefix = "Cargo",
            Options = [],
            PositionalArguments = usage.PositionalArguments,
            UsageSynopsis = usage.Synopsis,
            HasOperandTakingUsage = usage.HasOperandTokens,
        };
        var tool = new CliToolDefinition
        {
            ToolName = "cargo",
            NamespacePrefix = "Cargo",
            TargetNamespace = "ModularPipelines.Cargo",
            OutputDirectory = "src/ModularPipelines.Cargo",
            Commands = [command],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;

        await Assert.That(generated).Contains(
            "PrependOptionTerminator = true");
    }

    [Test]
    public async Task Generator_Emits_Terminated_Passthrough_Before_Late_Operands()
    {
        var usage = UsageSynopsisParser.Parse(
            "Usage: gh codespace cp [-- [<scp flags>...]] <sources>... <dest>",
            ["gh", "codespace", "cp"]);
        var command = new CliCommandDefinition
        {
            FullCommand = "gh codespace cp",
            CommandParts = ["codespace", "cp"],
            ClassName = "GhCodespaceCpOptions",
            ParentClassName = "GhOptions",
            ToolNamespacePrefix = "Gh",
            Options = [],
            PositionalArguments = usage.PositionalArguments,
            UsageSynopsis = usage.Synopsis,
            HasOperandTakingUsage = usage.HasOperandTokens,
        };
        var tool = new CliToolDefinition
        {
            ToolName = "gh",
            NamespacePrefix = "Gh",
            TargetNamespace = "ModularPipelines.GitHub",
            OutputDirectory = "src/ModularPipelines.GitHub",
            Commands = [command],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;

        await Assert.That(generated).Contains(
            "CliArgument(0, Phase = CommandLinePhase.Passthrough, PrependOptionTerminator = true)");
        await Assert.That(generated).Contains(
            "CliArgument(0, Phase = CommandLinePhase.LateOperand, Required = true)");
        await Assert.That(generated).Contains(
            "CliArgument(1, Phase = CommandLinePhase.LateOperand, Required = true)");
    }

    [Test]
    public async Task Model_Rejects_OperandTaking_Usage_With_No_Positionals()
    {
        var command = new CliCommandDefinition
        {
            FullCommand = "tool broken",
            CommandParts = ["broken"],
            ClassName = "ToolBrokenOptions",
            ParentClassName = "ToolOptions",
            ToolNamespacePrefix = "Tool",
            Options = [],
            UsageSynopsis = "tool broken <TARGET>",
            HasOperandTakingUsage = true,
        };

        await Assert.That(command.ValidateOperandCoverage)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("no CliPositionalArgument");
    }

    [Test]
    public async Task Model_Accepts_Usage_Operand_Covered_By_Named_Option()
    {
        var usage = UsageSynopsisParser.Parse(
            "Usage: winget search [[-q] <query>] [<options>]",
            ["winget", "search"]);
        var command = new CliCommandDefinition
        {
            FullCommand = "winget search",
            CommandParts = ["search"],
            ClassName = "WingetSearchOptions",
            ParentClassName = "WingetOptions",
            ToolNamespacePrefix = "Winget",
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--query",
                    ShortForm = "-q",
                    PropertyName = "Query",
                    CSharpType = "string?",
                },
            ],
        };

        command.ValidateOperandCoverage(
            usage.HasOperandTokens,
            usage.Synopsis,
            usage.PositionalArguments);

        await Assert.That(usage.PositionalArguments.Single().PropertyName)
            .IsEqualTo("Query");
        await Assert.That(usage.PositionalArguments.Single().AssociatedOptionSwitch)
            .IsEqualTo("-q");
    }

    [Test]
    public async Task Model_Accepts_Multiword_Usage_Operand_Covered_By_Named_Option()
    {
        var usage = UsageSynopsisParser.Parse(
            "Usage: grype explain --id [VULNERABILITY ID] [flags]",
            ["grype", "explain"]);
        var command = new CliCommandDefinition
        {
            FullCommand = "grype explain",
            CommandParts = ["explain"],
            ClassName = "GrypeExplainOptions",
            ParentClassName = "GrypeOptions",
            ToolNamespacePrefix = "Grype",
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--id",
                    PropertyName = "Id",
                    CSharpType = "string?",
                },
            ],
        };

        command.ValidateOperandCoverage(
            usage.HasOperandTokens,
            usage.Synopsis,
            usage.PositionalArguments);

        using (Assert.Multiple())
        {
            await Assert.That(usage.PositionalArguments.Single().PropertyName)
                .IsEqualTo("VulnerabilityId");
            await Assert.That(usage.PositionalArguments.Single().AssociatedOptionSwitch)
                .IsEqualTo("--id");
        }
    }

    [Test]
    public async Task Associates_Standalone_Option_Value_With_Its_Switch()
    {
        var usage = UsageSynopsisParser.Parse(
            "Usage: kubectl diff -f FILENAME",
            ["kubectl", "diff"]);

        using (Assert.Multiple())
        {
            await Assert.That(usage.PositionalArguments.Single().PropertyName)
                .IsEqualTo("Filename");
            await Assert.That(usage.PositionalArguments.Single().AssociatedOptionSwitch)
                .IsEqualTo("-f");
        }
    }

    [Test]
    public async Task Model_Rejects_Standalone_Operand_Sharing_Named_Option_Property()
    {
        var usage = UsageSynopsisParser.Parse(
            "Usage: tool run [--output OUTPUT] OUTPUT",
            ["tool", "run"]);
        var command = new CliCommandDefinition
        {
            FullCommand = "tool run",
            CommandParts = ["run"],
            ClassName = "ToolRunOptions",
            ParentClassName = "ToolOptions",
            ToolNamespacePrefix = "Tool",
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--output",
                    PropertyName = "Output",
                    CSharpType = "string?",
                },
            ],
        };

        void Validate() => command.ValidateOperandCoverage(
            usage.HasOperandTokens,
            usage.Synopsis,
            usage.PositionalArguments);

        await Assert.That(Validate)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("no CliPositionalArgument");
    }

    [Test]
    [Arguments("--output", "--output")]
    [Arguments("[--output]", null)]
    public async Task Model_Rejects_Operand_After_SameNamed_Boolean_Option(
        string optionUsage,
        string? associatedOptionSwitch)
    {
        var usage = UsageSynopsisParser.Parse(
            $"Usage: tool run {optionUsage} OUTPUT",
            ["tool", "run"]);
        var command = new CliCommandDefinition
        {
            FullCommand = "tool run",
            CommandParts = ["run"],
            ClassName = "ToolRunOptions",
            ParentClassName = "ToolOptions",
            ToolNamespacePrefix = "Tool",
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--output",
                    PropertyName = "Output",
                    CSharpType = "bool?",
                    IsFlag = true,
                },
            ],
        };

        void Validate() => command.ValidateOperandCoverage(
            usage.HasOperandTokens,
            usage.Synopsis,
            usage.PositionalArguments);

        using (Assert.Multiple())
        {
            await Assert.That(usage.PositionalArguments.Single().AssociatedOptionSwitch)
                .IsEqualTo(associatedOptionSwitch);
            await Assert.That(Validate)
                .Throws<InvalidOperationException>()
                .And.HasMessageContaining("no CliPositionalArgument");
        }
    }

    private static OperandFixture Fixture(
        string tool,
        string helpText,
        string[] commandPath,
        params ExpectedArgument[] expectedArguments) =>
        new(tool, helpText, commandPath, expectedArguments);

    private static ExpectedArgument Required(
        string propertyName,
        bool isVariadic = false,
        CommandLinePhase phase = CommandLinePhase.EarlyOperand) =>
        new(propertyName, IsRequired: true, IsVariadic: isVariadic, Phase: phase);

    private static ExpectedArgument Optional(
        string propertyName,
        bool isVariadic = false,
        CommandLinePhase phase = CommandLinePhase.EarlyOperand) =>
        new(propertyName, IsRequired: false, IsVariadic: isVariadic, Phase: phase);

    private sealed record OperandFixture(
        string Tool,
        string HelpText,
        string[] CommandPath,
        IReadOnlyList<ExpectedArgument> ExpectedArguments);

    private sealed record ExpectedArgument(
        string PropertyName,
        bool IsRequired,
        bool IsVariadic,
        CommandLinePhase Phase);

    private sealed class TestKustomizeCliScraper : KustomizeCliScraper
    {
        public TestKustomizeCliScraper()
            : base(
                Executor(),
                Cache(),
                NullLogger<KustomizeCliScraper>.Instance)
        {
        }

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }

        public Task<CliCommandDefinition?> ParseWithoutUsage(string[] commandPath, string helpText) =>
            ParseCommandAsync(commandPath, helpText, CancellationToken.None);
    }

    private sealed class TestNewmanCliScraper : NewmanCliScraper
    {
        public TestNewmanCliScraper()
            : base(
                Executor(),
                Cache(),
                NullLogger<NewmanCliScraper>.Instance)
        {
        }

        public IReadOnlyList<string> Extract(string helpText) => [.. ExtractSubcommands(helpText)];
    }

    private sealed class CountingUsageScraper : CliScraperBase
    {
        public CountingUsageScraper()
            : base(
                new StubExecutor(),
                Cache(),
                NullLogger<CountingUsageScraper>.Instance)
        {
        }

        public int UsageParseCount { get; private set; }

        public UsageSynopsisParseResult ParseUsage(string[] commandPath, string helpText) =>
            ParseUsageSynopsis(commandPath, helpText);

        public override string ToolName => "tool";

        public override string NamespacePrefix => "Tool";

        public override string TargetNamespace => "ModularPipelines.Tool";

        public override string OutputDirectory => "src/ModularPipelines.Tool";

        protected override IEnumerable<string> ExtractSubcommands(string helpText) => [];

        protected override IEnumerable<string> GetAdditionalUsageSynopses(
            string[] commandPath,
            string helpText)
        {
            UsageParseCount++;
            return [];
        }

        protected override Task<CliCommandDefinition?> ParseCommandAsync(
            string[] commandPath,
            string helpText,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Shared traversal should pass its parsed synopsis.");

        protected override Task<CliCommandDefinition?> ParseCommandAsync(
            string[] commandPath,
            string helpText,
            UsageSynopsisParseResult usage,
            CancellationToken cancellationToken) =>
            Task.FromResult<CliCommandDefinition?>(new CliCommandDefinition
            {
                FullCommand = "tool",
                CommandParts = [],
                ClassName = "ToolOptions",
                ParentClassName = "ToolOptions",
                ToolNamespacePrefix = "Tool",
                Options = [],
                PositionalArguments = usage.PositionalArguments,
                UsageSynopsis = usage.Synopsis,
                HasOperandTakingUsage = usage.HasOperandTokens,
            });
    }

    private sealed class StubExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = "Usage: tool <TARGET>",
                StandardError = string.Empty,
            });

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private static ProcessCliCommandExecutor Executor() =>
        new(NullLogger<ProcessCliCommandExecutor>.Instance);

    private static HelpTextCache Cache() =>
        new(NullLogger<HelpTextCache>.Instance);
}
