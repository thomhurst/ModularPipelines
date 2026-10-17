using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class CobraCliScraperTests
{
    [Test]
    [Arguments("int", "int?")]
    [Arguments("int8", "int?")]
    [Arguments("int16", "int?")]
    [Arguments("int32", "int?")]
    [Arguments("int64", "long?")]
    [Arguments("uint", "ulong?")]
    [Arguments("uint8", "uint?")]
    [Arguments("uint16", "uint?")]
    [Arguments("uint32", "uint?")]
    [Arguments("uint64", "ulong?")]
    [Arguments("float32", "double?")]
    [Arguments("float64", "double?")]
    [Arguments("number", "string?")]
    [Arguments("count", "int?")]
    [Arguments("integer", "string?")]
    [Arguments("number64", "string?")]
    [Arguments("real", "string?")]
    [Arguments("decimal", "string?")]
    [Arguments("double", "string?")]
    [Arguments("float", "double?")]
    [Arguments("ip", "string?")]
    [Arguments("duration", "string?")]
    public async Task Pflag_Numeric_Help_Types_Produce_Numeric_Options(string hint, string expectedType)
    {
        var help = $"Usage: fake run [flags]\n\nFlags:\n  --value {hint}   Value to use.\n";
        var command = await new TestCobraCliScraper().Parse(["fake", "run"], help);
        var option = command!.Options.Single();

        await Assert.That(option.CSharpType).IsEqualTo(expectedType);
        await Assert.That(option.IsNumeric).IsEqualTo(expectedType is "int?" or "long?" or "uint?" or "ulong?" or "double?");
        await Assert.That(option.AcceptsMultipleValues).IsFalse();
        await Assert.That(option.IsFlag).IsFalse();
    }

    [Test]
    [Arguments("strings")]
    [Arguments("ints")]
    [Arguments("uints")]
    [Arguments("bools")]
    [Arguments("stringSlice")]
    [Arguments("stringArray")]
    [Arguments("intSlice")]
    [Arguments("int32Slice")]
    [Arguments("int64Slice")]
    [Arguments("uintSlice")]
    [Arguments("boolSlice")]
    [Arguments("float32Slice")]
    [Arguments("float64Slice")]
    [Arguments("durationSlice")]
    [Arguments("ipSlice")]
    [Arguments("customSlice")]
    [Arguments("customArray")]
    [Arguments("IPSLICE")]
    [Arguments("STRINGARRAY")]
    [Arguments("list")]
    [Arguments("array")]
    public async Task Pflag_Collections_Produce_Repeatable_String_Options(string hint)
    {
        var help = $"Usage: fake run [flags]\n\nFlags:\n  --value {hint}   Values to use.\n";
        var command = await new TestCobraCliScraper().Parse(["fake", "run"], help);
        var option = command!.Options.Single();

        await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(option.AcceptsMultipleValues).IsTrue();
        await Assert.That(option.IsNumeric).IsFalse();
        await Assert.That(option.IsFlag).IsFalse();
    }

    [Test]
    public async Task Floating_Point_Defaults_Remain_Numeric()
    {
        const string help = "Usage: fake run [flags]\n\nOptions:\n  --ratio=1.5: Scaling ratio.\n";
        var command = await new TestCobraCliScraper().Parse(["fake", "run"], help);

        await Assert.That(command!.Options.Single().CSharpType).IsEqualTo("double?");
        await Assert.That(command.Options.Single().IsNumeric).IsTrue();
    }

    [Test]
    [Arguments("Gh/issue-edit.txt", "--add-sub-issue", "string?", false)]
    [Arguments("Gh/issue-edit.txt", "--add-blocked-by", "string?", false)]
    [Arguments("Gh/issue-edit.txt", "--parent", "string?", false)]
    [Arguments("Helm/3.22.0/install.txt", "--post-renderer-args", "IEnumerable<string>?", true)]
    [Arguments("Helm/3.22.0/template.txt", "--post-renderer-args", "IEnumerable<string>?", true)]
    [Arguments("Podman/6.1.3/container-exec-help.txt", "--preserve-fd", "IEnumerable<string>?", true)]
    [Arguments("Podman/6.1.3/container-exec-help.txt", "--preserve-fds", "ulong?", false)]
    [Arguments("Podman/6.1.3/container-run-help.txt", "--ip", "string?", false)]
    [Arguments("Podman/6.1.3/container-run-help.txt", "--cpus", "double?", false)]
    [Arguments("Trivy/0.75.0/image.txt", "--license-confidence-level", "double?", false)]
    [Arguments("Syft/1.54.0/root-help.txt", "--verbose", "int?", false)]
    [Arguments("Grype/0.120.0/grype-help.txt", "--verbose", "int?", false)]
    public async Task Real_Help_Preserves_Placeholders_And_Collection_Types(
        string fixture, string switchName, string expectedType, bool acceptsMultipleValues)
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", fixture));
        var option = new TestCobraCliScraper().Options(help).Single(option => option.SwitchName == switchName);

        await Assert.That(option.CSharpType).IsEqualTo(expectedType);
        await Assert.That(option.AcceptsMultipleValues).IsEqualTo(acceptsMultipleValues);
        await Assert.That(option.IsFlag).IsFalse();
    }

    [Test]
    [Arguments("--image=image", true)]
    [Arguments("--image IMAGE", true)]
    [Arguments("[--image=image]", false)]
    [Arguments("(--image=image | --filename=FILE)", false)]
    [Arguments("[flags]\n  fake run --image=image", false)]
    public async Task Only_Unconditional_Synopsis_Options_Are_Required(string synopsis, bool required)
    {
        var help = $"Usage:\n  fake run {synopsis}\n\nOptions:\n  --image strings    Container images.\n  --filename string  Input file.\n";
        var command = await new TestCobraCliScraper().Parse(["fake", "run"], help);
        await Assert.That(command!.Options.Single(option => option.SwitchName == "--image").IsRequired).IsEqualTo(required);
        await Assert.That(command.Options.Single(option => option.SwitchName == "--filename").IsRequired).IsFalse();
    }

    [Test]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public async Task Blank_Paragraphs_Preserve_Aligned_Prose_Without_Consuming_Options_Or_Footer(string newline)
    {
        const string help = """
            Usage: fake run [OPTIONS]

            Options:
                  --tmpdir string   Temporary state directory.

                                    Use TMPDIR for image storage.

                  --output string   Output path.

                    See the manual for examples.
            """;
        var command = await new TestCobraCliScraper().Parse(["fake", "run"], help.ReplaceLineEndings(newline));
        await Assert.That(command!.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--tmpdir", "--output"]);
        await Assert.That(command.Options.Single(option => option.SwitchName == "--tmpdir").Description)
            .IsEqualTo("Temporary state directory. Use TMPDIR for image storage.");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--output").Description)
            .IsEqualTo("Output path.");
    }

    [Test]
    [Arguments("--registries.d DIR    Registry directory.")]
    [Arguments("--registries.d='/etc/containers/registries.d': Registry directory.")]
    public async Task Dotted_Switches_Preserve_Spelling_And_Generate_Valid_Property_Names(string declaration)
    {
        var help = $"Usage: fake inspect [flags]\n\nFlags:\n  {declaration}\n  --next string    Next option.\n";
        var command = await new TestCobraCliScraper().Parse(["fake", "inspect"], help);
        var option = command!.Options.Single(option => option.SwitchName == "--registries.d");
        await Assert.That(option.PropertyName).IsEqualTo("RegistriesD");
        await Assert.That(option.CSharpType).IsEqualTo("string?");
        await Assert.That(option.Description).IsEqualTo("Registry directory.");
        await Assert.That(command.Options.Select(option => option.SwitchName)).IsEquivalentTo(["--registries.d", "--next"]);
    }

    [Test]
    public async Task Empty_Short_Alias_Description_Does_Not_Consume_Indented_Option_Rows()
    {
        const string help = """
            Usage: fake run [OPTIONS]

            Options:
              -h, --help=false:
                  --legacy=true: Preserve legacy behavior.
                  --trace=:0: Stop at file:N.
                  --output='': Output path.
            """;
        var command = await new TestCobraCliScraper().Parse(["fake", "run"], help);

        await Assert.That(command!.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--help", "--legacy", "--trace", "--output"]);
        await Assert.That(command.Options.Single(option => option.SwitchName == "--help").Description).IsEmpty();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--trace").Description)
            .IsEqualTo("Stop at file:N.");
    }

    [Test]
    [Arguments("[https://storage.example/image.iso,https://mirror.example/image.iso]")]
    [Arguments("[http://[::1]:8080/image.iso,https://mirror.example/image.iso]")]
    [Arguments("[]")]
    public async Task Bracketed_Default_Lists_Stay_Out_Of_Descriptions(string defaultValue)
    {
        var helpText = $"""
            Usage: fake start [OPTIONS]

            Options:
                  --iso-url={defaultValue}: Locations to fetch the ISO from.
                  --nodes=1: Number of nodes.
            """;
        var command = await new TestCobraCliScraper().Parse(["fake", "start"], helpText);

        var urls = command!.Options.Single(option => option.SwitchName == "--iso-url");
        await Assert.That(urls.Description)
            .IsEqualTo("Locations to fetch the ISO from.");
        await Assert.That(urls.AcceptsMultipleValues).IsTrue();
        await Assert.That(urls.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--nodes").Description)
            .IsEqualTo("Number of nodes.");
    }

    [Test]
    [Arguments("'", "\n")]
    [Arguments("\"", "\n")]
    [Arguments("'", "\r\n")]
    [Arguments("\"", "\r\n")]
    public async Task Multiline_Quoted_Defaults_Preserve_Descriptions_And_Adjacent_Options(string quote, string newline)
    {
        var helpText = $"""
            Usage: fake status [OPTIONS]

            Options:
              --before=1: Previous option.
              -f, --format={quote}name: name-value
            Template:
              --literal template content
            host: host-value{quote}: Render the template.
                Preserve this wrapped description.
              --after=false: Following option.

            Global Flags:
              --verbose   Show more details
            """.ReplaceLineEndings(newline);
        var command = await new TestCobraCliScraper().Parse(["fake", "status"], helpText);
        var format = command!.Options.Single(option => option.SwitchName == "--format");

        await Assert.That(command.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--before", "--format", "--after", "--verbose"]);
        await Assert.That(format.ShortForm).IsEqualTo("-f");
        await Assert.That(format.CSharpType).IsEqualTo("string?");
        await Assert.That(format.IsFlag).IsFalse();
        await Assert.That(format.Description)
            .IsEqualTo("Render the template. Preserve this wrapped description.");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--after").Description)
            .IsEqualTo("Following option.");
        var namedOptions = new TestCobraCliScraper().NamedOptions(helpText);
        await Assert.That(namedOptions.Select(option => option.SwitchName))
            .IsEquivalentTo(["--before", "--format", "--after"]);
        await Assert.That(namedOptions.Single(option => option.SwitchName == "--format").Description)
            .IsEqualTo(format.Description);
    }

    [Test]
    [Arguments("Templates use <key>=<value>")]
    [Arguments("Reference <kind>/<name>, where kind must be one of: Secret, ConfigMap")]
    [Arguments("Reference <kind>/<name>, where kind has allowed values: - Secret - ConfigMap")]
    public async Task Type_Hint_Preserves_Choices_Alongside_Composite_Prose(string description)
    {
        var helpText = $"Usage: fake run [OPTIONS]\n\nOptions:\n  --mode Foo|Bar   {description}\n";
        var command = await new TestCobraCliScraper().Parse(["fake", "run"], helpText);

        await Assert.That(command!.Options.Single().EnumDefinition).IsNotNull();
        await Assert.That(command.Options.Single().EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["Foo", "Bar"]);
    }

    [Test]
    [Arguments("Allowed values: json, yaml; templates use <key>=<value>")]
    [Arguments("Templates use <key>=<value>. Allowed values: json, yaml")]
    [Arguments("Templates use <key>=<value>. Allowed values: - json - yaml")]
    public async Task Description_Preserves_Whole_Value_Choices_Alongside_Composite_Prose(string description)
    {
        var helpText = $"Usage: fake run [OPTIONS]\n\nOptions:\n  --mode string   {description}\n";
        var command = await new TestCobraCliScraper().Parse(["fake", "run"], helpText);

        await Assert.That(command!.Options.Single().EnumDefinition).IsNotNull();
        await Assert.That(command.Options.Single().EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["json", "yaml"]);
    }

    [Test]
    public async Task Description_Rejects_Component_Bullet_Choices()
    {
        const string helpText = "Usage: fake run [OPTIONS]\n\nOptions:\n  --reference string   Reference <kind>/<name>, where kind has allowed values: - Secret - ConfigMap\n";
        var command = await new TestCobraCliScraper().Parse(["fake", "run"], helpText);

        await Assert.That(command!.Options.Single().EnumDefinition).IsNull();
        await Assert.That(command.Options.Single().CSharpType).IsEqualTo("string?");
    }

    [Test]
    public async Task Type_Hint_Preserves_Long_Choices()
    {
        const string longValue = "this-is-a-valid-choice-longer-than-thirty-characters";
        var helpText = $"Usage: fake run [OPTIONS]\n\nOptions:\n  --output-mode plain|json|{longValue}   Select mode\n";
        var command = await new TestCobraCliScraper().Parse(["fake", "run"], helpText);
        var definition = command!.Options.Single().EnumDefinition!;

        await Assert.That(definition.Values.Select(value => value.CliValue)).IsEquivalentTo(["plain", "json", longValue]);
        await Assert.That(definition.Description).IsEqualTo("Allowed values for --output-mode.");
    }

    [Test]
    [Arguments("<json|yaml>")]
    [Arguments("{json|yaml}")]
    [Arguments("[json|yaml]")]
    [Arguments("(json|yaml)")]
    public async Task Type_Hint_Strips_Choice_Wrappers(string hint)
    {
        var helpText = $"Usage: fake run [OPTIONS]\n\nOptions:\n  --mode {hint}   Select mode\n";
        var command = await new TestCobraCliScraper().Parse(["fake", "run"], helpText);

        await Assert.That(command!.Options.Single().EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["json", "yaml"]);
    }

    [Test]
    public async Task Large_Type_Hint_Choice_Set_Remains_In_Description()
    {
        var valueHint = string.Join('|', Enumerable.Range(1, 21).Select(index => $"mode{index}"));
        var helpText = $"Usage: fake run [OPTIONS]\n\nOptions:\n  --mode {valueHint}   Select mode\n";
        var command = await new TestCobraCliScraper().Parse(["fake", "run"], helpText);
        var option = command!.Options.Single();

        await Assert.That(option.EnumDefinition).IsNull();
        await Assert.That(option.CSharpType).IsEqualTo("string?");
        await Assert.That(option.Description).IsEqualTo($"Select mode [value type: {valueHint}]");
    }

    [Test]
    public async Task Explicit_Enum_Choices_Preserve_Case_And_Normalization_Collisions()
    {
        const string helpText = """
            Usage: fake run [OPTIONS]

            Options:
              --mode string   Allowed values: plain, PLAIN, foo-bar, foo_bar
            """;
        var command = await new TestCobraCliScraper().Parse(["fake", "run"], helpText);
        var definition = command!.Options.Single().EnumDefinition!;

        await Assert.That(definition.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["plain", "PLAIN", "foo-bar", "foo_bar"]);
        await Assert.That(definition.Values.Select(value => value.MemberName).Distinct().Count())
            .IsEqualTo(definition.Values.Count);
    }

    [Test]
    public async Task Repeatable_Noun_Phrases_Produce_Collection_Options()
    {
        const string helpText = """
            Initialize a service

            Usage: fake service init [OPTIONS]

            Options:
              --external-ca external-ca   Specifications of one or more certificate signing endpoints
            """;
        var command = await new TestCobraCliScraper().Parse(
            ["fake", "service", "init"],
            helpText);

        var option = command!.Options.Single();
        using (Assert.Multiple())
        {
            await Assert.That(option.AcceptsMultipleValues).IsTrue();
            await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
        }
    }

    [Test]
    public async Task Quoted_Kubectl_Style_Defaults_Containing_Colons_Stay_Out_Of_Descriptions()
    {
        // minikube (kubectl-style rows) prints the default inside quotes; the value itself may
        // contain colons, which must not be mistaken for the description separator.
        const string helpText = """
            Starts a local Kubernetes cluster

            Usage: fake start [OPTIONS]

            Options:
                  --base-image='gcr.io/k8s-minikube/kicbase:v0.0.51@sha256:4a1c825b61479e6c898851ea66f13c620aaeab6002746e95067fc2c4b38a0b24': The base image to use for docker/podman drivers. Intended for local development.
                  --iso-url='[https://storage.googleapis.com/minikube/iso/minikube-v1.39.0-amd64.iso,https://github.com/kubernetes/minikube/releases/download/v1.39.0/minikube-v1.39.0-amd64.iso]': Locations to fetch the minikube ISO from.
                  --kvm-qemu-uri='qemu:///system': The KVM QEMU connection URI. (kvm2 driver only)
                  --memory='': Amount of RAM to allocate to Kubernetes (format: <number>[<unit>], where unit = b, k, m or g).
                  --nodes=1: The total number of nodes to spin up.
            """;
        var command = await new TestCobraCliScraper().Parse(["fake", "start"], helpText);

        string Description(string switchName) =>
            command!.Options.Single(option => option.SwitchName == switchName).Description!;

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--base-image", "--iso-url", "--kvm-qemu-uri", "--memory", "--nodes"]);
            await Assert.That(Description("--base-image"))
                .IsEqualTo("The base image to use for docker/podman drivers. Intended for local development.");
            await Assert.That(Description("--iso-url"))
                .IsEqualTo("Locations to fetch the minikube ISO from.");
            await Assert.That(Description("--kvm-qemu-uri"))
                .IsEqualTo("The KVM QEMU connection URI. (kvm2 driver only)");
            await Assert.That(Description("--memory"))
                .IsEqualTo("Amount of RAM to allocate to Kubernetes (format: <number>[<unit>], where unit = b, k, m or g).");
            await Assert.That(command.Options.Single(option => option.SwitchName == "--kvm-qemu-uri").CSharpType)
                .IsEqualTo("string?");
            await Assert.That(command.Options.Single(option => option.SwitchName == "--nodes").CSharpType)
                .IsEqualTo("int?");
        }
    }

    [Test]
    public async Task Short_Command_Descriptions_Are_Preserved()
    {
        const string helpText = """
            Usage: fake report usage

            Disk usage

            Options:
              --verbose   Show more details
            """;
        var command = await new TestCobraCliScraper().Parse(
            ["fake", "report", "usage"],
            helpText);

        await Assert.That(command!.Description).IsEqualTo("Disk usage");
    }

    [Test]
    public async Task Usage_Continuations_Are_Not_Command_Descriptions()
    {
        const string helpText = """
            Usage:
              fake report usage

            Disk usage

            Options:
              --verbose   Show more details
            """;
        var command = await new TestCobraCliScraper().Parse(
            ["fake", "report", "usage"],
            helpText);

        await Assert.That(command!.Description).IsEqualTo("Disk usage");
    }

    [Test]
    [Arguments("Usage:\n  fake run [flags]")]
    [Arguments("Available Commands:\n  child    Child command")]
    [Arguments("Flags:\n  --trace    Trace execution")]
    [Arguments("Aliases:\n  run, execute")]
    [Arguments("Examples:\n  $ fake run")]
    public async Task Wrapped_Description_Stops_Before_Sections(string section)
    {
        var help = "Run this command with\n  wrapped documentation.\n" + section
            + "\n\nUsage:\n  fake run [flags]";
        var command = await new TestCobraCliScraper().Parse(["fake", "run"], help);
        await Assert.That(command!.Description).IsEqualTo("Run this command with wrapped documentation.");
    }

    [Test]
    public async Task Description_Stops_After_First_Paragraph()
    {
        var help = "First paragraph\n  continues here.\n\nSecond paragraph.\n\nUsage:\n  fake run [flags]";
        var command = await new TestCobraCliScraper().Parse(["fake", "run"], help);
        await Assert.That(command!.Description).IsEqualTo("First paragraph continues here.");
    }

    private sealed class TestCobraCliScraper : CobraCliScraper
    {
        public TestCobraCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<TestCobraCliScraper>.Instance)
        {
        }

        public override string ToolName => "fake";

        public override string NamespacePrefix => "Fake";

        public override string TargetNamespace => "ModularPipelines.Fake";

        public override string OutputDirectory => "src/ModularPipelines.Fake";

        public IReadOnlyList<CliOptionDefinition> Options(string helpText) => ParseOptions(helpText, []);

        public IReadOnlyList<CliOptionDefinition> NamedOptions(string helpText) =>
            ParseNamedOptionSection(helpText, "Options", ["status"]);

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }
}
