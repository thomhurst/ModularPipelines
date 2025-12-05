using ModularPipelines.Attributes;
using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.UnitTests.Attributes;

public class CliAttributeTests
{
    private readonly CommandModelProvider _modelProvider = new();
    private readonly CommandArgumentBuilder _argumentBuilder = new();

    private List<string> BuildArguments(object optionsObject)
    {
        var model = _modelProvider.GetCommandModel(optionsObject.GetType());
        return _argumentBuilder.BuildArguments(model, optionsObject);
    }

    [Test]
    public async Task CliCommand_Returns_Tool_And_SubCommands()
    {
        var attribute = new CliCommandAttribute("helm", "install");
        var parts = attribute.GetAllParts();

        await Assert.That(parts).IsEquivalentTo(new[] { "helm", "install" });
    }

    [Test]
    public async Task CliCommand_Returns_Only_Tool_When_No_SubCommands()
    {
        var attribute = new CliCommandAttribute("helm");
        var parts = attribute.GetAllParts();

        await Assert.That(parts).IsEquivalentTo(new[] { "helm" });
    }

    [Test]
    public async Task CliCommand_Returns_Multiple_SubCommands()
    {
        var attribute = new CliCommandAttribute("kubectl", "get", "pods");
        var parts = attribute.GetAllParts();

        await Assert.That(parts).IsEquivalentTo(new[] { "kubectl", "get", "pods" });
    }

    [Test]
    public async Task CliFlag_Returns_Name_When_ShortForm_Not_Preferred()
    {
        var attribute = new CliFlagAttribute("--debug") { ShortForm = "-d" };

        await Assert.That(attribute.GetEffectiveName()).IsEqualTo("--debug");
    }

    [Test]
    public async Task CliFlag_Returns_ShortForm_When_Preferred()
    {
        var attribute = new CliFlagAttribute("--debug") { ShortForm = "-d", PreferShortForm = true };

        await Assert.That(attribute.GetEffectiveName()).IsEqualTo("-d");
    }

    [Test]
    public async Task CliFlag_Returns_Name_When_ShortForm_Null_And_Preferred()
    {
        var attribute = new CliFlagAttribute("--debug") { PreferShortForm = true };

        await Assert.That(attribute.GetEffectiveName()).IsEqualTo("--debug");
    }

    [Test]
    [Arguments(OptionFormat.SpaceSeparated, " ")]
    [Arguments(OptionFormat.EqualsSeparated, "=")]
    [Arguments(OptionFormat.ColonSeparated, ":")]
    [Arguments(OptionFormat.NoSeparator, "")]
    public async Task CliOption_GetSeparator_Returns_Correct_Separator(OptionFormat format, string expected)
    {
        var attribute = new CliOptionAttribute("--namespace") { Format = format };

        await Assert.That(attribute.GetSeparator()).IsEqualTo(expected);
    }

    [Test]
    public async Task CliOption_CustomSeparator_Overrides_Format()
    {
        var attribute = new CliOptionAttribute("--namespace")
        {
            Format = OptionFormat.SpaceSeparated,
            CustomSeparator = "::",
        };

        await Assert.That(attribute.GetSeparator()).IsEqualTo("::");
    }

    [Test]
    public async Task CliOption_Returns_Name_When_ShortForm_Not_Preferred()
    {
        var attribute = new CliOptionAttribute("--namespace") { ShortForm = "-n" };

        await Assert.That(attribute.GetEffectiveName()).IsEqualTo("--namespace");
    }

    [Test]
    public async Task CliOption_Returns_ShortForm_When_Preferred()
    {
        var attribute = new CliOptionAttribute("--namespace") { ShortForm = "-n", PreferShortForm = true };

        await Assert.That(attribute.GetEffectiveName()).IsEqualTo("-n");
    }

    [Test]
    public async Task CliArgument_Defaults_To_AfterOptions_Placement()
    {
        var attribute = new CliArgumentAttribute(0);

        await Assert.That(attribute.Placement).IsEqualTo(ArgumentPlacement.AfterOptions);
    }

    [Test]
    public async Task CliArgument_Position_Is_Set_Correctly()
    {
        var attribute = new CliArgumentAttribute(2);

        await Assert.That(attribute.Position).IsEqualTo(2);
    }

    [Test]
    public async Task Parser_Handles_CliFlag()
    {
        var options = new TestCliOptionsWithFlag { Debug = true };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "--debug" });
    }

    [Test]
    public async Task Parser_Omits_CliFlag_When_False()
    {
        var options = new TestCliOptionsWithFlag { Debug = false };
        var list = BuildArguments(options);

        await Assert.That(list).Count().IsEqualTo(0);
    }

    [Test]
    public async Task Parser_Omits_CliFlag_When_Null()
    {
        var options = new TestCliOptionsWithFlag { Debug = null };
        var list = BuildArguments(options);

        await Assert.That(list).Count().IsEqualTo(0);
    }

    [Test]
    public async Task Parser_Handles_CliOption_With_Space_Separator()
    {
        var options = new TestCliOptionsWithOption { Namespace = "default" };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "--namespace", "default" });
    }

    [Test]
    public async Task Parser_Handles_CliOption_With_Equals_Separator()
    {
        var options = new TestCliOptionsWithEqualsSeparator { Set = "key=value" };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "--set=key=value" });
    }

    [Test]
    public async Task Parser_Handles_CliOption_With_Multiple_Values()
    {
        var options = new TestCliOptionsWithMultipleValues { Values = ["file1.yaml", "file2.yaml"] };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "--values", "file1.yaml", "--values", "file2.yaml" });
    }

    [Test]
    public async Task Parser_Handles_CliArgument_After_Options()
    {
        var options = new TestCliOptionsWithArgumentAfterOptions
        {
            ReleaseName = "myrelease",
            Debug = true,
        };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "--debug", "myrelease" });
    }

    [Test]
    public async Task Parser_Handles_CliArgument_Before_Options()
    {
        var options = new TestCliOptionsWithArgumentBeforeOptions
        {
            Path = "/some/path",
            Debug = true,
        };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "/some/path", "--debug" });
    }

    [Test]
    public async Task Parser_Omits_Null_CliArgument()
    {
        var options = new TestCliOptionsWithOptionalArgument { ReleaseName = null, Debug = true };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "--debug" });
    }

    [Test]
    public async Task Parser_Orders_Multiple_Arguments_By_Position()
    {
        var options = new TestCliOptionsWithMultipleArguments
        {
            ReleaseName = "myrelease",
            ChartReference = "bitnami/nginx",
        };
        var list = BuildArguments(options);

        await Assert.That(list).IsEquivalentTo(new[] { "myrelease", "bitnami/nginx" });
    }

    [Test]
    public async Task Parser_Handles_Mixed_Flags_Options_And_Arguments()
    {
        var options = new TestCliOptionsComplete
        {
            ReleaseName = "myrelease",
            ChartReference = "bitnami/nginx",
            Namespace = "production",
            Debug = true,
            Set = ["key1=val1", "key2=val2"],
        };
        var list = BuildArguments(options);

        await Assert.That(list[0]).IsEqualTo("--debug");
        await Assert.That(list).Contains("--namespace");
        await Assert.That(list).Contains("production");
        await Assert.That(list).Contains("--set=key1=val1");
        await Assert.That(list).Contains("--set=key2=val2");
        await Assert.That(list).Contains("myrelease");
        await Assert.That(list).Contains("bitnami/nginx");
    }

    // Test option classes
    private record TestCliOptionsWithFlag
    {
        [CliFlag("--debug")]
        public bool? Debug { get; set; }
    }

    private record TestCliOptionsWithOption
    {
        [CliOption("--namespace")]
        public string? Namespace { get; set; }
    }

    private record TestCliOptionsWithEqualsSeparator
    {
        [CliOption("--set", Format = OptionFormat.EqualsSeparated)]
        public string? Set { get; set; }
    }

    private record TestCliOptionsWithMultipleValues
    {
        [CliOption("--values", AllowMultiple = true)]
        public string[]? Values { get; set; }
    }

    private record TestCliOptionsWithArgumentAfterOptions
    {
        [CliArgument(0)]
        public string? ReleaseName { get; set; }

        [CliFlag("--debug")]
        public bool? Debug { get; set; }
    }

    private record TestCliOptionsWithArgumentBeforeOptions
    {
        [CliArgument(0, Placement = ArgumentPlacement.BeforeOptions)]
        public string? Path { get; set; }

        [CliFlag("--debug")]
        public bool? Debug { get; set; }
    }

    private record TestCliOptionsWithOptionalArgument
    {
        [CliArgument(0)]
        public string? ReleaseName { get; set; }

        [CliFlag("--debug")]
        public bool? Debug { get; set; }
    }

    private record TestCliOptionsWithMultipleArguments
    {
        [CliArgument(0)]
        public string? ReleaseName { get; set; }

        [CliArgument(1)]
        public string? ChartReference { get; set; }
    }

    private record TestCliOptionsComplete
    {
        [CliArgument(0)]
        public string? ReleaseName { get; set; }

        [CliArgument(1)]
        public string? ChartReference { get; set; }

        [CliFlag("--debug")]
        public bool? Debug { get; set; }

        [CliOption("--namespace")]
        public string? Namespace { get; set; }

        [CliOption("--set", Format = OptionFormat.EqualsSeparated, AllowMultiple = true)]
        public string[]? Set { get; set; }
    }
}
