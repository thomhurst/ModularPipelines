using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    public async Task Aws_Numeric_Hints_Preserve_Fractions_And_Long_Values()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "AwsCli", "numeric-options.txt"));
        var command = (await new NumericAwsScraper().Parse(help))!;
        await ValidateNumericOptions(command,
        [
            ("Threshold", "double?", 1.25d),
            ("ContentLength", "long?", 2147483648L),
            ("Ratio", "double?", 0.125d),
            ("Count", "int?", 42),
        ]);
    }

    [Test]
    public async Task Cobra_Numeric_Hints_Preserve_Widths_Signedness_And_Fractions()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Cobra", "numeric-options.txt"));
        var command = (await new NumericCobraScraper().Parse(help))!;
        await ValidateNumericOptions(command,
        [
            ("Count", "int?", 42),
            ("LimitBytes", "long?", 2147483648L),
            ("Offset", "long?", -2147483649L),
            ("Size", "ulong?", ulong.MaxValue),
            ("UnsignedCount", "uint?", uint.MaxValue),
            ("NativeUnsigned", "ulong?", ulong.MaxValue),
            ("Ratio", "double?", 0.125d),
            ("Threshold", "double?", 1.25d),
            ("LargeDefault", "long?", 2147483648L),
            ("UnsignedDefault", "ulong?", ulong.MaxValue),
        ]);
    }

    [Test]
    public async Task Kubectl_Value_Only_Help_Preserves_Declared_Log_Integer_Widths()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Kubectl", "numeric-logs-options.txt"));
        var command = (await new NumericKubectlScraper().Parse(help))!;
        await ValidateNumericOptions(command,
        [
            ("LimitBytes", "long?", 2147483648L),
            ("Tail", "long?", 2147483648L),
            ("MaxLogRequests", "int?", 5),
        ]);
    }

    private static async Task ValidateNumericOptions(CliCommandDefinition command,
        (string PropertyName, string CSharpType, object Value)[] expected)
    {
        var generated = await Generate([.. command.Options]);
        var type = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(type)!;
        await Assert.That(command.Options.Count).IsEqualTo(expected.Length);
        foreach (var (propertyName, cSharpType, value) in expected)
        {
            var option = command.Options.Single(option => option.PropertyName == propertyName);
            await Assert.That(option.CSharpType).IsEqualTo(cSharpType);
            await Assert.That(option.IsNumeric).IsTrue();
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.AcceptsMultipleValues).IsFalse();
            await Assert.That(generated).Contains($"public {cSharpType} {propertyName}");
            var property = type.GetProperty(propertyName)!;
            await Assert.That(Nullable.GetUnderlyingType(property.PropertyType)).IsEqualTo(value.GetType());
            property.SetValue(instance, value);
            var arguments = new CommandArgumentBuilder().BuildArguments(
                [new OptionPart(propertyName, property.GetValue, new CliOptionAttribute(option.SwitchName))], instance);
            await Assert.That(arguments).IsEquivalentTo(
                [option.SwitchName, ((IFormattable) value).ToString(null, System.Globalization.CultureInfo.InvariantCulture)],
                TUnit.Assertions.Enums.CollectionOrdering.Matching);
        }
    }

    private sealed class NumericAwsScraper() : AwsCliScraper(
        new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<AwsCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string help) =>
            ParseCommandAsync(["aws", "example", "run"], help,
                ParseUsageSynopsis(["aws", "example", "run"], help), CancellationToken.None);
    }

    private sealed class NumericCobraScraper() : CobraCliScraper(
        new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<CobraCliScraper>.Instance)
    {
        public override string ToolName => "fake";
        public override string NamespacePrefix => "Fake";
        public override string TargetNamespace => "ModularPipelines.Fake";
        public override string OutputDirectory => "unused";

        public Task<CliCommandDefinition?> Parse(string help) =>
            ParseCommandAsync(["fake", "run"], help,
                ParseUsageSynopsis(["fake", "run"], help), CancellationToken.None);
    }

    private sealed class NumericKubectlScraper() : KubectlCliScraper(
        new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<KubectlCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string help) =>
            ParseCommandAsync(["kubectl", "logs"], help,
                ParseUsageSynopsis(["kubectl", "logs"], help), CancellationToken.None);
    }
}
