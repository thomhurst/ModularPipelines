namespace ModularPipelines.SourceGenerator.UnitTests;

public class CommandPropertyReuseTests
{
    private const string Infrastructure = """
        namespace ModularPipelines.Options
        {
            public abstract class CommandLineToolOptions;
        }

        namespace ModularPipelines.Attributes
        {
            [System.AttributeUsage(System.AttributeTargets.Property)]
            public sealed class CliOptionAttribute(string name) : System.Attribute;
        }
        """;

    [Test]
    [Arguments(2, 14)]
    [Arguments(65, 14)]
    [Arguments(65, 65)]
    public async Task Inherited_Properties_Emit_One_Getter_And_Attribute_Per_Declaration(
        int derivedCount,
        int propertyCount)
    {
        var properties = string.Join(Environment.NewLine, Enumerable.Range(0, propertyCount)
            .Select(index => $$"""
                [ModularPipelines.Attributes.CliOption("--wide{{index}}")]
                public virtual string? Wide{{index}} { get; set; }
                """));
        var derivedTypes = string.Join(Environment.NewLine, Enumerable.Range(0, derivedCount)
            .Select(index => $$"""
                public sealed class Options{{index}} : WideOptions
                {
                    [ModularPipelines.Attributes.CliOption("--own")]
                    public string? Own { get; set; }
                }
                """));
        var result = GeneratorTestHarness.Run(
            new CommandOptionsGenerator(),
            Infrastructure,
            $$"""
            public abstract class WideOptions : ModularPipelines.Options.CommandLineToolOptions
            {
                {{properties}}
            }
            {{derivedTypes}}
            """);
        var generated = string.Join(Environment.NewLine, result.GeneratedTrees);

        await Assert.That(result.Diagnostics).IsEmpty();
        await Assert.That(result.GeneratedTrees.SelectMany(static tree => tree.GetDiagnostics())).IsEmpty();
        for (var index = 0; index < propertyCount; index++)
        {
            await Assert.That(Count(generated, $"static instance => ((global::WideOptions)instance).@Wide{index},"))
                .IsEqualTo(1);
            await Assert.That(Count(generated, $"CliOptionAttribute(\"--wide{index}\")"))
                .IsEqualTo(1);
        }

        await Assert.That(Count(generated, "CliOptionAttribute(\"--own\")")).IsEqualTo(derivedCount);
    }

    [Test]
    public async Task External_Base_Properties_Reuse_Declaring_Type_Getters()
    {
        var result = GeneratorTestHarness.RunWithExternalAssembly(
            new CommandOptionsGenerator(),
            Infrastructure,
            """
            public abstract class ExternalOptions : ModularPipelines.Options.CommandLineToolOptions
            {
                [ModularPipelines.Attributes.CliOption("--project")]
                public string? Project { get; set; }
            }
            """,
            """
            public sealed class FirstOptions : ExternalOptions;
            public sealed class SecondOptions : ExternalOptions;
            """,
            globalOptions: new Dictionary<string, string>
            {
                ["build_property.PublishTrimmed"] = "true",
            });
        var generated = string.Join(Environment.NewLine, result.GeneratedTrees);

        await Assert.That(result.Diagnostics).IsEmpty();
        await Assert.That(Count(generated, "static instance => ((global::ExternalOptions)instance).@Project,"))
            .IsEqualTo(1);
        await Assert.That(Count(generated, "CliOptionAttribute(\"--project\")")).IsEqualTo(1);
        await Assert.That(generated).Contains("GeneratedCommandMetadata.RegisterExternal(");
    }

    private static int Count(string source, string value) =>
        source.Split(value, StringSplitOptions.None).Length - 1;
}
