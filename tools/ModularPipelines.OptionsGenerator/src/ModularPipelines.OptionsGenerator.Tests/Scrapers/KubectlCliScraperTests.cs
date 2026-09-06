using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class KubectlCliScraperTests
{
    [Test]
    public async Task Uses_Version_Subcommand_For_Availability_And_Version()
    {
        var executor = new RecordingExecutor();
        var scraper = new KubectlCliScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<KubectlCliScraper>.Instance);

        var isAvailable = await scraper.IsAvailableAsync();
        var version = await scraper.GetVersionAsync();

        await Assert.That(isAvailable).IsTrue();
        await Assert.That(version).IsEqualTo("Client Version: v1.35.0");
        await Assert.That(executor.AvailabilityArguments).IsEquivalentTo(["version --client"]);
        await Assert.That(executor.Arguments).IsEquivalentTo(["version --client"]);
    }

    [Test]
    [Arguments("diff", "Usage:\n  kubectl diff -f FILENAME", "-f, --filename stringArray   Files to diff.")]
    [Arguments("attach", "Usage:\n  kubectl attach POD -c CONTAINER", "-c, --container string   Container name.")]
    public async Task Option_Values_Are_Not_Emitted_As_Positionals(
        string commandName,
        string usage,
        string option)
    {
        var helpText = $"{usage}\n\nOptions:\n  {option}";
        var command = await new TestKubectlCliScraper().Parse(
            ["kubectl", commandName],
            helpText);

        await Assert.That(command!.PositionalArguments.Select(argument => argument.PropertyName))
            .DoesNotContain("Filename")
            .And.DoesNotContain("Container");
    }

    [Test]
    public async Task Port_Forward_Numbered_Repeat_Is_One_Collection()
    {
        const string helpText = """
            Usage:
              kubectl port-forward TYPE/NAME [options] [LOCAL_PORT:]REMOTE_PORT [...[LOCAL_PORT_N:]REMOTE_PORT_N]
            """;
        var command = await new TestKubectlCliScraper().Parse(
            ["kubectl", "port-forward"],
            helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.PositionalArguments.Select(argument => argument.PropertyName))
                .IsEquivalentTo(["TypeOrName", "LocalPortRemotePort"]);
            await Assert.That(command.PositionalArguments.Single(argument =>
                argument.PropertyName == "LocalPortRemotePort").IsVariadic).IsTrue();
        }
    }

    [Test]
    public async Task Auth_Can_I_Allows_List_Without_Verb()
    {
        const string helpText = """
            Usage:
              kubectl auth can-i VERB [TYPE | TYPE/NAME | NONRESOURCEURL] [options]
            """;

        var command = await new TestKubectlCliScraper().Parse(
            ["kubectl", "auth", "can-i"],
            helpText);

        var verb = command!.PositionalArguments.Single(argument => argument.PropertyName == "Verb");
        await Assert.That(verb.IsRequired).IsTrue();
        await Assert.That(verb.IsValidationRequired).IsFalse();
    }

    [Test]
    public async Task Annotate_Allows_List_Without_Annotations()
    {
        const string helpText = """
            Usage:
              kubectl annotate [--overwrite] (-f FILENAME | TYPE NAME) KEY_1=VAL_1 ... KEY_N=VAL_N [options]

            Options:
              -f, --filename stringArray   Files identifying resources.
                  --list                    Display annotations.
            """;

        var command = await new TestKubectlCliScraper().Parse(
            ["kubectl", "annotate"],
            helpText);

        var annotations = command!.PositionalArguments.Single(argument =>
            argument.PropertyName == "Annotations");
        await Assert.That(annotations.IsRequired).IsTrue();
        await Assert.That(annotations.IsValidationRequired).IsFalse();
    }

    [Test]
    [Arguments("cordon")]
    [Arguments("drain")]
    [Arguments("uncordon")]
    public async Task Node_Commands_Allow_Selector_Without_Node(string commandName)
    {
        var helpText = $"Usage:\n  kubectl {commandName} NODE [options]";
        var command = await new TestKubectlCliScraper().Parse(
            ["kubectl", commandName],
            helpText);

        var node = command!.PositionalArguments.Single(argument => argument.PropertyName == "Node");
        await Assert.That(node.IsRequired).IsTrue();
        await Assert.That(node.IsValidationRequired).IsFalse();
    }

    [Test]
    public async Task Logs_Allows_Selector_Without_Pod()
    {
        const string helpText = "Usage:\n  kubectl logs POD [options]";
        var command = await new TestKubectlCliScraper().Parse(
            ["kubectl", "logs"],
            helpText);

        var pod = command!.PositionalArguments.Single(argument => argument.PropertyName == "Pod");
        await Assert.That(pod.IsRequired).IsTrue();
        await Assert.That(pod.IsValidationRequired).IsFalse();
    }

    [Test]
    [Arguments("history", "(TYPE NAME | TYPE/NAME)", "TypeName")]
    [Arguments("pause", "RESOURCE", "Resource")]
    [Arguments("restart", "RESOURCE", "Resource")]
    [Arguments("resume", "RESOURCE", "Resource")]
    [Arguments("status", "(TYPE NAME | TYPE/NAME)", "TypeName")]
    [Arguments("undo", "(TYPE NAME | TYPE/NAME)", "TypeName")]
    public async Task Rollout_Commands_Allow_File_Without_Resource(
        string commandName,
        string usageOperand,
        string propertyName)
    {
        var helpText = $"Usage:\n  kubectl rollout {commandName} {usageOperand} [options]";
        var command = await new TestKubectlCliScraper().Parse(
            ["kubectl", "rollout", commandName],
            helpText);

        var resource = command!.PositionalArguments.Single(argument =>
            argument.PropertyName == propertyName);
        await Assert.That(resource.IsRequired).IsTrue();
        await Assert.That(resource.IsValidationRequired).IsFalse();
    }

    [Test]
    public async Task Events_Does_Not_Treat_Output_Short_Form_As_Operand()
    {
        const string helpText = """
            Usage:
              kubectl events [(-o|--output=)json|yaml|name] [options]

            Options:
              -o, --output string   Output format.
            """;
        var scraper = new TestKubectlCliScraper();
        var command = await scraper.Parse(
            ["kubectl", "events"],
            helpText);
        var usage = scraper.Normalize(
            command!,
            UsageSynopsisParser.Parse(helpText, ["kubectl", "events"]));

        await Assert.That(command!.PositionalArguments).IsEmpty();
        await Assert.That(usage.HasOperandTokens).IsFalse();
        await Assert.That(usage.UnparsedOperandTokens).IsEmpty();
    }

    [Test]
    public async Task Debug_Command_Is_Optional_With_Variadic_Arguments()
    {
        const string helpText = """
            Usage:
              kubectl debug (POD | TYPE[[.VERSION].GROUP]/NAME) [ -- COMMAND [args...] ] [options]
            """;

        var command = await new TestKubectlCliScraper().Parse(
            ["kubectl", "debug"],
            helpText);

        var passthrough = command!.PositionalArguments
            .Where(argument => argument.Phase == ModularPipelines.Attributes.CommandLinePhase.Passthrough)
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(passthrough.Select(argument => argument.PropertyName))
                .IsEquivalentTo(["CommandArgs", "Args"]);
            await Assert.That(passthrough.Any(argument =>
                argument.IsValidationRequired ?? argument.IsRequired)).IsFalse();
            await Assert.That(passthrough.Single(argument => argument.PropertyName == "Args").IsVariadic)
                .IsTrue();
        }
    }

    [Test]
    [Arguments("debug", "(POD | TYPE[[.VERSION].GROUP]/NAME)")]
    [Arguments("exec", "POD -- COMMAND [args...]")]
    public async Task Target_Commands_Allow_Filename_Without_Pod(
        string commandName,
        string usageOperand)
    {
        var helpText = $"""
            Usage:
              kubectl {commandName} {usageOperand} [options]

            Options:
              -f, --filename stringArray   File identifying the target resource.
            """;

        var command = await new TestKubectlCliScraper().Parse(
            ["kubectl", commandName],
            helpText);

        var pod = command!.PositionalArguments.Single(argument => argument.PropertyName == "Pod");
        await Assert.That(pod.IsRequired).IsTrue();
        await Assert.That(pod.IsValidationRequired).IsFalse();
    }

    [Test]
    public async Task Label_Allows_List_Without_Labels()
    {
        const string helpText = """
            Usage:
              kubectl label [--overwrite] (-f FILENAME | TYPE NAME) KEY_1=VAL_1 ... KEY_N=VAL_N [options]

            Options:
              -f, --filename stringArray   Files identifying resources.
            """;

        var command = await new TestKubectlCliScraper().Parse(
            ["kubectl", "label"],
            helpText);

        var labels = command!.PositionalArguments.Single(argument => argument.PropertyName == "Key_1Val_1");
        var trailingLabel = command.PositionalArguments.Single(argument => argument.PropertyName == "KeyNValN");
        using (Assert.Multiple())
        {
            await Assert.That(labels.IsRequired).IsTrue();
            await Assert.That(labels.IsValidationRequired).IsFalse();
            await Assert.That(labels.IsVariadic).IsTrue();
            await Assert.That(labels.CSharpType).IsEqualTo("IEnumerable<string>");
            await Assert.That(trailingLabel.IsValidationRequired).IsFalse();
            await Assert.That(command.PositionalArguments.Count(argument =>
                argument.IsValidationRequired ?? argument.IsRequired))
                .IsEqualTo(0);
        }
    }

    [Test]
    public async Task Taint_Allows_All_Without_Node_Name()
    {
        const string helpText = """
            Usage:
              kubectl taint NODE NAME KEY_1=VAL_1:TAINT_EFFECT_1 ... KEY_N=VAL_N:TAINT_EFFECT_N [options]
            """;

        var command = await new TestKubectlCliScraper().Parse(
            ["kubectl", "taint"],
            helpText);

        var name = command!.PositionalArguments.Single(argument => argument.PropertyName == "Name");
        var taints = command.PositionalArguments.Single(argument => argument.PropertyName == "Taints");
        using (Assert.Multiple())
        {
            await Assert.That(name.IsRequired).IsTrue();
            await Assert.That(name.IsValidationRequired).IsFalse();
            await Assert.That(taints.IsRequired).IsTrue();
            await Assert.That(taints.IsVariadic).IsTrue();
        }
    }

    [Test]
    public async Task Kuberc_Set_Option_Values_Do_Not_Keep_Operand_Usage()
    {
        const string helpText = """
            Usage:
              kubectl kuberc set --section (defaults|aliases) --command COMMAND [options]

            Options:
                  --section string   Section to update.
                  --command string   Command to update.
            """;
        var commandPath = new[] { "kubectl", "kuberc", "set" };
        var scraper = new TestKubectlCliScraper();
        var command = await scraper.Parse(commandPath, helpText);
        var usage = scraper.Normalize(
            command!,
            UsageSynopsisParser.Parse(helpText, commandPath));

        using (Assert.Multiple())
        {
            await Assert.That(command!.PositionalArguments).IsEmpty();
            await Assert.That(usage.PositionalArguments).IsEmpty();
            await Assert.That(usage.HasOperandTokens).IsFalse();
        }
    }

    [Test]
    public async Task Tab_Indented_Descriptions_Are_Joined_Across_Lines()
    {
        var helpText = string.Join(
            "\n",
            "Usage:",
            "  kubectl get TYPE [flags] [options]",
            "",
            "Options:",
            "    -A, --all-namespaces=false:",
            "\tIf present, list the requested object(s) across all namespaces. Namespace in current context is ignored even",
            "\tif specified with --namespace.",
            "",
            "    --allow-missing-template-keys=true:",
            "\tIf true, ignore any errors in templates when a field or map key is missing in the template.");

        var command = await new TestKubectlCliScraper().Parse(["kubectl", "get"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--all-namespaces", "--allow-missing-template-keys"]);
            await Assert.That(command.Options.Single(option => option.SwitchName == "--all-namespaces").Description)
                .IsEqualTo(
                    "If present, list the requested object(s) across all namespaces. Namespace in current context is ignored even if specified with --namespace.");
            await Assert.That(command.Options.Single(option => option.SwitchName == "--allow-missing-template-keys").Description)
                .IsEqualTo("If true, ignore any errors in templates when a field or map key is missing in the template.");
        }
    }

    private sealed class TestKubectlCliScraper()
        : KubectlCliScraper(
            new RecordingExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<KubectlCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                UsageSynopsisParser.Parse(helpText, commandPath),
                CancellationToken.None);

        public UsageSynopsisParseResult Normalize(
            CliCommandDefinition command,
            UsageSynopsisParseResult usage) =>
            NormalizeUsageSynopsis(command, usage);
    }

    private sealed class RecordingExecutor : ICliCommandExecutor
    {
        public List<string> Arguments { get; } = [];

        public List<string> AvailabilityArguments { get; } = [];

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            Arguments.Add(arguments);
            var success = arguments == "version --client";
            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = success ? "Client Version: v1.35.0" : string.Empty,
                StandardError = success ? string.Empty : "error: unknown flag: --version",
                ExitCode = success ? 0 : 1,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(false);

        public Task<bool> IsAvailableAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default)
        {
            AvailabilityArguments.Add(arguments);
            return Task.FromResult(arguments == "version --client");
        }
    }
}
