using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class EnumDefinitionStabilizerTests
{
    [Test]
    public async Task Stabilize_Preserves_Existing_Raw_Value_Order_And_Ordinals()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-enum-tests", Guid.NewGuid().ToString("N"));
        var enumDirectory = Path.Combine(outputRoot, "src", "Fake", "Enums");
        Directory.CreateDirectory(enumDirectory);
        File.WriteAllText(
            Path.Combine(enumDirectory, "FakeVisibility.Generated.cs"),
            """
            public enum FakeVisibility
            {
                [Description("private")]
                Private,

                [Description("public")]
                Public,

                [Description("internal")]
                Internal
            }
            """);

        try
        {
            var tool = Tool(
                Value("public"),
                Value("internal"),
                Value("private"),
                Value("enterprise"));

            var stabilized = EnumDefinitionStabilizer.Stabilize(tool, outputRoot);
            var values = stabilized.AllEnums.Single().Values;

            await Assert.That(values[0].CliValue).IsEqualTo("private");
            await Assert.That(values[1].CliValue).IsEqualTo("public");
            await Assert.That(values[2].CliValue).IsEqualTo("internal");
            await Assert.That(values[3].CliValue).IsEqualTo("enterprise");
            await Assert.That(values[0].NumericValue).IsEqualTo(0);
            await Assert.That(values[1].NumericValue).IsEqualTo(1);
            await Assert.That(values[2].NumericValue).IsEqualTo(2);
            await Assert.That(values[3].NumericValue).IsEqualTo(3);

            var generated = (await new EnumGenerator().GenerateAsync(stabilized)).Single().Content;
            await Assert.That(generated).Contains("Private = 0,");
            await Assert.That(generated).Contains("Public = 1,");
            await Assert.That(generated).Contains("Internal = 2,");
            await Assert.That(generated).Contains("Enterprise = 3");
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task Stabilize_RoundTrips_EnumGenerator_Output()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-enum-tests", Guid.NewGuid().ToString("N"));
        var enumDirectory = Path.Combine(outputRoot, "src", "Fake", "Enums");
        Directory.CreateDirectory(enumDirectory);
        var enumPath = Path.Combine(enumDirectory, "FakeVisibility.Generated.cs");

        try
        {
            var initial = EnumDefinitionStabilizer.Stabilize(
                Tool(Value("public"), Value("private")),
                outputRoot);
            var initialGenerated = (await new EnumGenerator().GenerateAsync(initial)).Single().Content;
            await File.WriteAllTextAsync(enumPath, initialGenerated);

            var reordered = Tool(Value("private"), Value("public"), Value("enterprise"));
            var stabilized = EnumDefinitionStabilizer.Stabilize(reordered, outputRoot);
            var values = stabilized.AllEnums.Single().Values;

            await Assert.That(values[0].CliValue).IsEqualTo("public");
            await Assert.That(values[1].CliValue).IsEqualTo("private");
            await Assert.That(values[2].CliValue).IsEqualTo("enterprise");
            await Assert.That(values[0].NumericValue).IsEqualTo(0);
            await Assert.That(values[1].NumericValue).IsEqualTo(1);
            await Assert.That(values[2].NumericValue).IsEqualTo(2);

            var generated = (await new EnumGenerator().GenerateAsync(stabilized)).Single().Content;
            await File.WriteAllTextAsync(enumPath, generated);

            var restabilized = EnumDefinitionStabilizer.Stabilize(reordered, outputRoot);
            var regenerated = (await new EnumGenerator().GenerateAsync(restabilized)).Single().Content;

            await Assert.That(regenerated).IsEqualTo(generated);
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task Stabilize_Preserves_Existing_Member_Name_For_Same_Cli_Value()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-enum-tests", Guid.NewGuid().ToString("N"));
        var enumDirectory = Path.Combine(outputRoot, "src", "Fake", "Enums");
        Directory.CreateDirectory(enumDirectory);
        File.WriteAllText(
            Path.Combine(enumDirectory, "FakeVisibility.Generated.cs"),
            """
            public enum FakeVisibility
            {
                [EnumValue("controllerManager")]
                Controllermanager
            }
            """);

        try
        {
            var stabilized = EnumDefinitionStabilizer.Stabilize(
                Tool(Value("controllerManager")),
                outputRoot);

            await Assert.That(stabilized.AllEnums.Single().Values.Single().MemberName)
                .IsEqualTo("Controllermanager");
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    [Test]
    public async Task Stabilize_Rejects_Suspicious_Prose_Values()
    {
        var tool = Tool(Value("them"), Value("accepts"));
        void Stabilize() => EnumDefinitionStabilizer.Stabilize(tool, Path.GetTempPath());

        await Assert.That(Stabilize)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("suspicious prose value");
    }

    [Test]
    public async Task Stabilize_Rejects_Preserved_Member_Name_Colliding_With_New_Value()
    {
        var outputRoot = Path.Combine(Path.GetTempPath(), "mp-enum-tests", Guid.NewGuid().ToString("N"));
        var enumDirectory = Path.Combine(outputRoot, "src", "Fake", "Enums");
        Directory.CreateDirectory(enumDirectory);
        File.WriteAllText(
            Path.Combine(enumDirectory, "FakeVisibility.Generated.cs"),
            """
            public enum FakeVisibility
            {
                [EnumValue("legacy")]
                FutureValue
            }
            """);

        try
        {
            void Stabilize() => EnumDefinitionStabilizer.Stabilize(
                Tool(Value("legacy"), Value("future-value")),
                outputRoot);

            await Assert.That(Stabilize)
                .Throws<InvalidOperationException>()
                .And.HasMessageContaining("duplicate member name 'FutureValue' after stabilization");
        }
        finally
        {
            Directory.Delete(outputRoot, recursive: true);
        }
    }

    private static CliToolDefinition Tool(params CliEnumValue[] values)
    {
        var definition = new CliEnumDefinition
        {
            EnumName = "FakeVisibility",
            Values = values,
        };

        return new CliToolDefinition
        {
            ToolName = "fake",
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = Path.Combine("src", "Fake"),
            Commands =
            [
                new CliCommandDefinition
                {
                    FullCommand = "fake",
                    CommandParts = [],
                    ClassName = "FakeOptions",
                    ParentClassName = "FakeOptions",
                    ToolNamespacePrefix = "Fake",
                    Options = [],
                    Enums = [definition],
                },
            ],
        };
    }

    private static CliEnumValue Value(string cliValue) => new()
    {
        CliValue = cliValue,
        MemberName = GeneratorUtils.ToEnumMemberName(cliValue),
    };
}
