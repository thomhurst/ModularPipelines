using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class IgnoredOptionPolicyTests
{
    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    [Arguments(true, true)]
    public async Task Ignored_Global_Options_Preserve_Usage_Operand_Ownership(bool supplemental, bool isFlag)
    {
        var help = Option("--help", "Help") with { IsFlag = isFlag, CSharpType = isFlag ? "bool?" : "string?" };
        var scraper = new PolicyScraper([], globals: supplemental ? [] : [help],
            supplemental: supplemental ? [help] : [], synopsis: "--help <INPUT>",
            operands: isFlag ? [new() { PropertyName = "Input", CSharpType = "string", IsRequired = true, PositionIndex = 0 }] : [],
            useSubcommand: true);
        var command = (await Scrape(scraper)).Single();
        await Assert.That(command.FullCommand).IsEqualTo("probe child");
        await Assert.That(scraper.EffectiveGlobals).IsEmpty();
        await Assert.That(command.Options).IsEmpty();
        await Assert.That(command.UsagePositionalArguments).Count().IsEqualTo(isFlag ? 1 : 0);
        if (isFlag)
        {
            await Assert.That(command.UsagePositionalArguments.Single().AssociatedOptionSwitch).IsNull();
        }
    }

    [Test]
    public async Task Local_Ignored_Flag_Takes_Precedence_Over_Global_Value_Option()
    {
        var scraper = new PolicyScraper([Option("--help", "Help")],
            globals: [Option("--help", "Help") with { IsFlag = false, CSharpType = "string?" }],
            synopsis: "--help <INPUT>",
            operands: [new() { PropertyName = "Input", CSharpType = "string", IsRequired = true, PositionIndex = 0 }],
            useSubcommand: true);
        var command = (await Scrape(scraper)).Single();
        await Assert.That(command.UsagePositionalArguments.Single().PropertyName).IsEqualTo("Input");
        await Assert.That(command.UsagePositionalArguments.Single().AssociatedOptionSwitch).IsNull();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Alternative_Members_With_Colliding_Properties_Keep_Their_Identity(bool positional)
    {
        var retained = new CliRequiredAlternativeMember
        {
            PropertyName = "Help",
            OptionSwitch = positional ? null : "--output",
            PositionalArgumentPhase = positional ? CommandLinePhase.EarlyOperand : null,
            PositionalArgumentPositionIndex = positional ? 0 : null,
        };
        List<CliOptionDefinition> options = [Option("--help", "Help")];
        if (!positional)
        {
            options.Add(Option("--output", "Help"));
        }

        var scraper = new PolicyScraper(options,
            groups: [new() { Members = [new() { PropertyName = "Help", OptionSwitch = "--help" }, retained] }],
            operands: positional ? [new() { PropertyName = "Help", CSharpType = "string?", PositionIndex = 0, Phase = CommandLinePhase.EarlyOperand }] : []);
        var command = (await Scrape(scraper)).Single();
        await Assert.That(command.RequiredAlternativeGroups.Single().Members).IsEquivalentTo([retained]);
        var tool = InheritedPropertyCollisionResolver.Resolve(scraper.CreateToolDefinition() with { Commands = [command] });
        var generated = await new OptionsClassGenerator().GenerateAsync(tool);
        await Assert.That(generated).IsNotEmpty();
    }

    [Test]
    public async Task Ignored_Flag_Does_Not_Own_The_Following_Operand()
    {
        var scraper = new PolicyScraper([Option("--help", "Help")], synopsis: "--help <INPUT>",
            operands: [new() { PropertyName = "Input", CSharpType = "string", IsRequired = true, PositionIndex = 0 }]);
        var command = (await Scrape(scraper)).Single();
        await Assert.That(command.UsagePositionalArguments.Single().PropertyName).IsEqualTo("Input");
        await Assert.That(command.UsagePositionalArguments.Single().AssociatedOptionSwitch).IsNull();
        await Assert.That(command.HasOperandTakingUsage).IsTrue();
    }

    [Test]
    [Arguments("-h")]
    [Arguments("--no-help")]
    public async Task Ignored_Aliases_Are_Removed_From_Group_And_Operand_Metadata(string alias)
    {
        var ignored = Option("--help", "Help") with { ShortForm = "-h", NegatedSwitchName = "--no-help" };
        var scraper = new PolicyScraper([ignored, Option("--output", "Output")],
            new HashSet<string>(StringComparer.Ordinal) { alias },
            synopsis: $"{alias} <INPUT>",
            operands: [new() { PropertyName = "Input", CSharpType = "string", IsRequired = true, PositionIndex = 0 }],
            groups: [new() { Members = [new() { PropertyName = "Help", OptionSwitch = alias }, new() { PropertyName = "Output", OptionSwitch = "--output" }] }],
            argumentGroups: [new() { Arguments = [new() { SwitchName = alias }, new() { SwitchName = "--output" }] }]);
        var command = (await Scrape(scraper)).Single();
        await Assert.That(command.Options.Select(option => option.SwitchName)).IsEquivalentTo(["--output"]);
        await Assert.That(command.UsagePositionalArguments.Single().AssociatedOptionSwitch).IsNull();
        await Assert.That(command.RequiredAlternativeGroups.Single().Members.Single().OptionSwitch).IsEqualTo("--output");
        await Assert.That(command.ArgumentGroups.Single().Arguments.Single().SwitchName).IsEqualTo("--output");
    }

    [Test]
    public async Task Removing_Ignored_Values_Does_Not_Hide_Unmodeled_Operands()
    {
        var scraper = new PolicyScraper([Option("--help", "Help") with { CSharpType = "string?", IsFlag = false }],
            synopsis: "--help <FORMAT> <INPUT>");
        await Assert.That(await Scrape(scraper)).IsEmpty();
    }

    [Test]
    [Arguments("cargo", "build", "--manifest-path <PATH>", "--manifest-path")]
    [Arguments("pnpm", "audit", "--audit-level <AUDIT_LEVEL>", "--audit-level")]
    [Arguments("helm", "status", "-o, --output string", "--output")]
    [Arguments("nbgv", "cloud", "--ci-system <ci-system>", "--ci-system")]
    [Arguments("dotnet", "build", "-c, --configuration <CONFIGURATION>", "--configuration")]
    [Arguments("pnpm", "audit", "--audit-level <AUDIT_LEVEL>", "--audit-level", "-h")]
    [Arguments("dotnet", "build", "-c, --configuration <CONFIGURATION>", "--configuration", "-h")]
    [Arguments("dotnet", "build", "-c, --configuration <CONFIGURATION>", "--configuration", "--HELP")]
    [Arguments("dotnet", "build", "-c, --configuration <CONFIGURATION>", "--configuration", "-H")]
    [Arguments("cargo", "build", "--manifest-path <PATH>", "--manifest-path", "--help <FORMAT>", "[--help <FORMAT>]")]
    [Arguments("pnpm", "audit", "--audit-level <AUDIT_LEVEL>", "--audit-level", "--help <FORMAT>", "[--help <FORMAT>]")]
    [Arguments("cargo", "build", "--manifest-path <PATH>", "--manifest-path", "--help <FORMAT>", "--help <FORMAT>")]
    [Arguments("pnpm", "audit", "--audit-level <AUDIT_LEVEL>", "--audit-level", "--help <FORMAT>", "--help <FORMAT>")]
    [Arguments("pnpm", "audit", "--audit-level <AUDIT_LEVEL>", "--audit-level", "--help", "(--help | --audit-level)")]
    [Arguments("nbgv", "cloud", "--ci-system <ci-system>", "--ci-system", "--help", "(--help | --ci-system)")]
    [Arguments("dotnet", "build", "-c, --configuration <CONFIGURATION>", "--configuration", "--help", "(--help | --configuration)")]
    public async Task Help_Rows_Are_Filtered_Without_Losing_Commands_Or_Adding_Operands(
        string tool, string commandName, string valueDeclaration, string retainedSwitch, string helpDeclaration = "--help", string helpUsage = "")
    {
        var commandHeading = tool == "helm" ? "Available Commands" : "Commands";
        var optionHeading = tool == "helm" ? "Flags" : "Options";
        var rootHelp = $"Usage: {tool} [command]\n\n{commandHeading}:\n  {commandName}  Run the command.\n\n{optionHeading}:\n  -h, --help  Show help.\n";
        var commandHelp = $"Usage: {tool} {commandName} {helpUsage} [options]\n\n{optionHeading}:\n      {helpDeclaration}  Show help.\n      {valueDeclaration}  Set the value.\n";
        var executor = new FixtureExecutor(new Dictionary<string, string>
        {
            ["--help"] = rootHelp,
            [$"{commandName} --help"] = commandHelp,
            [$"help {commandName}"] = "OPTIONS\n    The fixture options accept one value each.\n",
        });
        var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        ICliScraper scraper = tool switch
        {
            "cargo" => new CargoCliScraper(executor, cache, NullLogger<CargoCliScraper>.Instance),
            "pnpm" => new PnpmCliScraper(executor, cache, NullLogger<PnpmCliScraper>.Instance),
            "helm" => new HelmCliScraper(executor, cache, NullLogger<HelmCliScraper>.Instance),
            "nbgv" => new NbgvCliScraper(executor, cache, NullLogger<NbgvCliScraper>.Instance),
            "dotnet" => new DotNetCliScraper(executor, cache, NullLogger<DotNetCliScraper>.Instance),
            _ => throw new ArgumentOutOfRangeException(nameof(tool)),
        };
        var commands = await Scrape(scraper);
        var command = commands.Single(command => command.FullCommand == $"{tool} {commandName}");
        string[] expected = tool == "dotnet" ? [retainedSwitch, "-p"] : [retainedSwitch];
        await Assert.That(command.Options.Select(option => option.SwitchName)).IsEquivalentTo(expected);
        await Assert.That(command.PositionalArguments).IsEmpty();
        await Assert.That(command.UsagePositionalArguments).IsEmpty();
        if (helpUsage.StartsWith('('))
        {
            await Assert.That(command.RequiredAlternativeGroups.Single().Members.Single().OptionSwitch).IsEqualTo(retainedSwitch);
        }
    }

    [Test]
    public async Task Default_Policy_Matches_Aliases_Exactly_And_Preserves_Other_Short_Switches()
    {
        var scraper = new PolicyScraper([
            Option("--help", "Help"),
            Option("--usage", "Usage") with { ShortForm = "--help" },
            Option("--assist", "Assist") with { NegatedSwitchName = "--help" },
            Option("--Help", "UpperHelp"),
            Option("--hostname", "Hostname") with { ShortForm = "-h", CSharpType = "string?", IsFlag = false },
            Option("-h", "HaltOnError"),
            Option("--header", "Header") with { ShortForm = "-H" },
        ]);
        var command = (await Scrape(scraper)).Single();
        await Assert.That(command.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--Help", "--hostname", "-h", "--header"]);
        await Assert.That(command.Options.Single(option => option.SwitchName == "--hostname").ShortForm).IsEqualTo("-h");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--header").ShortForm).IsEqualTo("-H");
    }

    [Test]
    public async Task Override_Can_Keep_Help_And_Ignore_A_Different_Alias()
    {
        var scraper = new PolicyScraper([
            Option("--help", "Help"),
            Option("--trace", "Trace") with { ShortForm = "-t" },
            Option("--toggle", "Toggle") with { ShortForm = "-T" },
        ], new HashSet<string>(StringComparer.Ordinal) { "-t" });
        var command = (await Scrape(scraper)).Single();
        await Assert.That(command.Options.Select(option => option.SwitchName)).IsEquivalentTo(["--help", "--toggle"]);
    }

    [Test]
    public async Task Policy_Applies_To_Scraped_And_Supplemental_Global_Options()
    {
        var scraper = new PolicyScraper([Option("--output", "Output")], globals: [Option("--help", "Help"), Option("--verbose", "Verbose")],
            supplemental: [Option("--usage", "Usage") with { ShortForm = "--help" }, Option("--profile", "Profile")]);
        await Scrape(scraper);
        var tool = scraper.CreateToolDefinition();
        await Assert.That(tool.GlobalOptions.Select(option => option.SwitchName)).IsEquivalentTo(["--verbose"]);
        await Assert.That(tool.SupplementalGlobalOptions.Select(option => option.SwitchName)).IsEquivalentTo(["--profile"]);
        await Assert.That(scraper.EffectiveGlobals.Select(option => option.SwitchName)).IsEquivalentTo(["--verbose", "--profile"]);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Nested_Alternative_Groups_Remove_Ignored_Members_And_Preserve_Surviving_Bundles(bool retainRootMember)
    {
        var ignoredMember = new CliRequiredAlternativeMember { PropertyName = "Help", OptionSwitch = "--help", IsRequired = true };
        var retainedMember = new CliRequiredAlternativeMember { PropertyName = "Output", OptionSwitch = "--output", IsRequired = true };
        var ignoredGroup = new CliRequiredAlternativeGroup { Members = [ignoredMember] };
        var bundle = new CliRequiredAlternativeGroup
        {
            IsChoice = false,
            Members = [ignoredMember, retainedMember],
            Groups = [ignoredGroup],
        };
        var choice = new CliRequiredAlternativeGroup
        {
            IsRequired = false,
            IsMutuallyExclusive = true,
            Members = retainRootMember
                ? [new() { PropertyName = "Mode", OptionSwitch = "--mode" }]
                : [ignoredMember],
            Groups = [bundle],
        };
        var scraper = new PolicyScraper(
            [Option("--help", "Help"), Option("--output", "Output"), Option("--mode", "Mode")],
            groups: [choice, new() { Members = [], Groups = [ignoredGroup] }]);

        var command = (await Scrape(scraper)).Single();
        var filteredChoice = command.RequiredAlternativeGroups.Single();
        await Assert.That(filteredChoice.IsRequired).IsFalse();
        await Assert.That(filteredChoice.IsChoice).IsTrue();
        await Assert.That(filteredChoice.IsMutuallyExclusive).IsTrue();
        await Assert.That(filteredChoice.Members.Select(member => member.PropertyName))
            .IsEquivalentTo(retainRootMember ? ["Mode"] : Array.Empty<string>());
        var filteredBundle = filteredChoice.Groups.Single();
        await Assert.That(filteredBundle.IsRequired).IsTrue();
        await Assert.That(filteredBundle.IsChoice).IsFalse();
        await Assert.That(filteredBundle.Members).IsEquivalentTo([retainedMember]);
        await Assert.That(filteredBundle.Groups).IsEmpty();
    }

    [Test]
    public async Task Ignored_Metadata_Is_Removed_While_Shared_Enums_And_Valid_Groups_Remain()
    {
        var discarded = new CliEnumDefinition { EnumName = "Discarded", Values = [] };
        var shared = new CliEnumDefinition { EnumName = "Shared", Values = [] };
        var ignoredGroup = new CliRequiredAlternativeGroup
        {
            Members = [new() { PropertyName = "Help", OptionSwitch = "--help" }, new() { PropertyName = "Output", OptionSwitch = "--output" }],
        };
        var allIgnoredGroup = new CliRequiredAlternativeGroup
        {
            Members = [new() { PropertyName = "Help", OptionSwitch = "--help" }, new() { PropertyName = "Hidden", OptionSwitch = "--hidden" }],
        };
        var retainedGroup = new CliRequiredAlternativeGroup
        {
            Members = [new() { PropertyName = "Mode", OptionSwitch = "--mode" }, new() { PropertyName = "Output", OptionSwitch = "--output" }],
        };
        var scraper = new PolicyScraper([
            Option("--help", "Help") with { CSharpType = "string?", IsFlag = false, EnumDefinition = discarded },
            Option("--hidden", "Hidden") with { CSharpType = "string?", IsFlag = false, EnumDefinition = shared },
            Option("--mode", "Mode") with { CSharpType = "string?", IsFlag = false, EnumDefinition = shared },
            Option("--output", "Output"),
        ], new HashSet<string>(StringComparer.Ordinal) { "--help", "--hidden" },
            enums: [discarded, shared], groups: [ignoredGroup, allIgnoredGroup, retainedGroup]);
        var command = (await Scrape(scraper)).Single();
        await Assert.That(command.Options.Select(option => option.SwitchName)).IsEquivalentTo(["--mode", "--output"]);
        await Assert.That(command.Enums).IsEquivalentTo([shared]);
        await Assert.That(command.RequiredAlternativeGroups).Count().IsEqualTo(2);
        await Assert.That(command.RequiredAlternativeGroups[0].PropertyNames).IsEquivalentTo(["Output"]);
        await Assert.That(command.RequiredAlternativeGroups[1].Members).IsEquivalentTo(retainedGroup.Members);
    }

    [Test]
    [Arguments("--help")]
    [Arguments("--usage")]
    public async Task Inferred_Groups_Preserve_Alternatives_When_An_Ignored_Switch_Or_Alias_Is_Removed(string ignoredSwitch)
    {
        var scraper = new PolicyScraper([
            Option(ignoredSwitch, "Help") with { ShortForm = "--help" },
            Option("--output", "Output"),
        ], synopsis: $"({ignoredSwitch} | --output)");
        var command = (await Scrape(scraper)).Single();
        await Assert.That(command.RequiredAlternativeGroups.Single().PropertyNames).IsEquivalentTo(["Output"]);
    }

    [Test]
    public async Task Enum_References_Without_Ownership_Are_Preserved()
    {
        var optionEnum = new CliEnumDefinition { EnumName = "OutputMode", Values = [] };
        var operandEnum = new CliEnumDefinition { EnumName = "InputMode", Values = [] };
        var scraper = new PolicyScraper([
            Option("--help", "Help") with { EnumDefinition = optionEnum },
            Option("--hidden", "Hidden") with { EnumDefinition = operandEnum },
            Option("--output", "Output") with { CSharpType = "IEnumerable<OutputMode>?", IsFlag = false },
        ], new HashSet<string>(StringComparer.Ordinal) { "--help", "--hidden" },
            enums: [optionEnum, operandEnum],
            operands: [new() { PropertyName = "Input", CSharpType = "InputMode?" }]);
        var command = (await Scrape(scraper)).Single();
        await Assert.That(command.Enums).IsEquivalentTo([optionEnum, operandEnum]);
    }

    [Test]
    [Arguments("--help", null, null)]
    [Arguments("--usage", "--help", null)]
    [Arguments("--assist", null, "--help")]
    public async Task Ignored_Options_Are_Removed_From_Nested_Argument_Groups(
        string switchName, string? shortForm, string? negatedSwitchName)
    {
        var retained = new CliArgumentDefinition { SwitchName = "--output", Description = "Output path.", SourceOrder = 7 };
        var scraper = new PolicyScraper([
            Option(switchName, "Help") with { ShortForm = shortForm, NegatedSwitchName = negatedSwitchName },
            Option("--output", "Output"),
        ], argumentGroups:
        [
            new()
            {
                Description = "Select an output.",
                Kind = CliArgumentGroupKind.AtLeastOne,
                Arguments = [new() { SwitchName = switchName }],
                Groups =
                [
                    new() { Arguments = [new() { SwitchName = switchName }] },
                    new() { Description = "Output settings.", Arguments = [retained] },
                ],
            },
            new() { Groups = [new() { Arguments = [new() { SwitchName = switchName }] }] },
        ]);

        var command = (await Scrape(scraper)).Single();
        var group = command.ArgumentGroups.Single();
        await Assert.That(group.Description).IsEqualTo("Select an output.");
        await Assert.That(group.Kind).IsEqualTo(CliArgumentGroupKind.AtLeastOne);
        await Assert.That(group.Arguments).IsEmpty();
        await Assert.That(group.Groups.Single().Description).IsEqualTo("Output settings.");
        await Assert.That(group.Groups.Single().Arguments.Single()).IsEqualTo(retained);
        await Assert.That(group.FlattenArguments().Single().Documentation)
            .IsEqualTo("Select an output. Output settings. Output path.");
    }

    private static CliOptionDefinition Option(string switchName, string propertyName) =>
        new() { SwitchName = switchName, PropertyName = propertyName, CSharpType = "bool?", IsFlag = true };

    private static async Task<List<CliCommandDefinition>> Scrape(ICliScraper scraper)
    {
        List<CliCommandDefinition> commands = [];
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        return commands;
    }

    private sealed class PolicyScraper(
        IReadOnlyList<CliOptionDefinition> options,
        IReadOnlySet<string>? ignored = null,
        IReadOnlyList<CliOptionDefinition>? globals = null,
        IReadOnlyList<CliOptionDefinition>? supplemental = null,
        IReadOnlyList<CliEnumDefinition>? enums = null,
        IReadOnlyList<CliRequiredAlternativeGroup>? groups = null,
        IReadOnlyList<CliPositionalArgument>? operands = null,
        string synopsis = "[options]",
        IReadOnlyList<CliArgumentGroup>? argumentGroups = null,
        bool useSubcommand = false)
        : CliScraperBase(new FixtureExecutor(useSubcommand
                ? new Dictionary<string, string>
                {
                    ["--help"] = "Usage: probe [command]\n\nCommands:\n  child  Run a command.\n",
                    ["child --help"] = $"Usage: probe child {synopsis}\n\nOptions:\n  --output  Select output.\n",
                }
                : new Dictionary<string, string> { ["--help"] = $"Usage: probe {synopsis}\n\nOptions:\n  --help  Show help.\n" }),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger.Instance)
    {
        public override string ToolName => "probe";
        public override string NamespacePrefix => "Probe";
        public override string TargetNamespace => "ModularPipelines.Probe";
        public override string OutputDirectory => "src/ModularPipelines.Probe";
        protected override IReadOnlySet<string> IgnoredOptionSwitches => ignored ?? base.IgnoredOptionSwitches;
        protected override IReadOnlyList<CliOptionDefinition> SupplementalGlobalOptions => supplemental ?? [];
        protected override IReadOnlyList<CliOptionDefinition> ParseGlobalOptions(string helpText) => globals ?? [];
        public IReadOnlyList<CliOptionDefinition> EffectiveGlobals => EffectiveGlobalOptions;

        protected override IEnumerable<string> ExtractSubcommands(string[] commandPath, string helpText) =>
            useSubcommand && commandPath.Length == 1 ? ["child"] : [];

        protected override Task<CliCommandDefinition?> ParseCommandAsync(string[] commandPath, string helpText, CancellationToken cancellationToken) =>
            Task.FromResult<CliCommandDefinition?>(useSubcommand && commandPath.Length == 1 ? null : new()
            {
                FullCommand = string.Join(" ", commandPath),
                CommandParts = commandPath.Skip(1).ToArray(),
                ClassName = "ProbeRunOptions",
                ParentClassName = "ProbeOptions",
                ToolNamespacePrefix = "Probe",
                Options = options,
                Enums = enums ?? [],
                RequiredAlternativeGroups = groups ?? [],
                PositionalArguments = operands ?? [],
                ArgumentGroups = argumentGroups ?? [],
            });
    }

    private sealed class FixtureExecutor(IReadOnlyDictionary<string, string> help) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments, CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            var found = help.TryGetValue(arguments, out var output);
            return Task.FromResult(new CliCommandResult { StandardOutput = output ?? "", StandardError = "", ExitCode = found ? 0 : 1 });
        }

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }
}
