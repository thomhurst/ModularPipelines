namespace ModularPipelines.SourceGenerator.UnitTests;

public class CommandOptionsSharedPartsTests
{
    private const string Infrastructure = """
        namespace ModularPipelines.Options
        {
            public abstract record CommandLineToolOptions;
        }

        namespace ModularPipelines.Attributes
        {
            [System.AttributeUsage(System.AttributeTargets.Property)]
            public sealed class CliOptionAttribute(string name) : System.Attribute;
        }

        namespace ModularPipelines.Secrets
        {
            [System.AttributeUsage(System.AttributeTargets.Property)]
            public sealed class SecretValueAttribute(params string[] keys) : System.Attribute;
        }
        """;

    [Test]
    public async Task Inherited_Parts_Are_Emitted_Once_And_Referenced_By_Each_Registration()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            Infrastructure,
            """
            public abstract record WideOptions : ModularPipelines.Options.CommandLineToolOptions
            {
                [ModularPipelines.Attributes.CliOption("--account")]
                public virtual string? Account { get; set; }

                [ModularPipelines.Secrets.SecretValue]
                public virtual string? Token { get; set; }
            }

            public sealed record FirstOptions : WideOptions
            {
                [ModularPipelines.Attributes.CliOption("--first")]
                public string? First { get; set; }
            }

            public sealed record SecondOptions : WideOptions
            {
                [ModularPipelines.Attributes.CliOption("--second")]
                public string? Second { get; set; }
            }
            """);

        var sources = result.GeneratedTrees.ToDictionary(
            static tree => Path.GetFileName(tree.FilePath),
            static tree => tree.ToString());
        var commandSource = sources["ModularPipelines.RuntimeMetadata.RuntimeMetadataSharedCommandParts0000.g.cs"];
        var secretSource = sources["ModularPipelines.RuntimeMetadata.RuntimeMetadataSharedSecretParts0000.g.cs"];
        var registrationSource = sources["ModularPipelines.RuntimeMetadata.g.cs"];
        var allSources = string.Concat(sources.Values);

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(commandSource).Contains("PropertyCommandLinePart Part0000 =");
            await Assert.That(commandSource).DoesNotContain("Part0001");
            await Assert.That(secretSource).Contains("SecretPropertyAccessor Part0000 =");
            await Assert.That(CountOccurrences(allSources, "((global::WideOptions)instance).@Account")).IsEqualTo(1);
            await Assert.That(CountOccurrences(allSources, "((global::WideOptions)instance).@Token")).IsEqualTo(1);

            // The abstract base registers too, so all three registrations reference the shared parts.
            await Assert.That(CountOccurrences(registrationSource, "RuntimeMetadataSharedCommandParts0000.Part0000,")).IsEqualTo(3);
            await Assert.That(CountOccurrences(registrationSource, "RuntimeMetadataSharedSecretParts0000.Part0000,")).IsEqualTo(3);
            await Assert.That(registrationSource).Contains("((global::FirstOptions)instance).@First");
            await Assert.That(registrationSource).Contains("((global::SecondOptions)instance).@Second");
        }
    }

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
                public sealed record Options{{index}} : WideOptions
                {
                    [ModularPipelines.Attributes.CliOption("--own")]
                    public string? Own { get; set; }
                }
                """));
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            Infrastructure,
            $$"""
            public abstract record WideOptions : ModularPipelines.Options.CommandLineToolOptions
            {
                {{properties}}
            }
            {{derivedTypes}}
            """);
        var generated = string.Concat(result.GeneratedTrees.Select(static tree => tree.ToString()));
        var commandPartFiles = result.GeneratedTrees
            .Select(static tree => Path.GetFileName(tree.FilePath))
            .Count(static name => name.Contains("RuntimeMetadataSharedCommandParts", StringComparison.Ordinal));

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(result.GeneratedTrees.SelectMany(static tree => tree.GetDiagnostics())).IsEmpty();
            for (var index = 0; index < propertyCount; index++)
            {
                await Assert.That(CountOccurrences(generated, $"static instance => ((global::WideOptions)instance).@Wide{index},"))
                    .IsEqualTo(1);
                await Assert.That(CountOccurrences(generated, $"CliOptionAttribute(\"--wide{index}\")"))
                    .IsEqualTo(1);
            }

            await Assert.That(CountOccurrences(generated, "CliOptionAttribute(\"--own\")")).IsEqualTo(derivedCount);

            // Shared fields are chunked like registrations: 32 parts per class.
            await Assert.That(commandPartFiles).IsEqualTo((propertyCount + 31) / 32);
        }
    }

    [Test]
    public async Task External_Base_Properties_Reuse_Declaring_Type_Getters()
    {
        var result = GeneratorTestHarness.RunWithExternalAssembly(
            new CommandOptionsGenerator(),
            Infrastructure,
            """
            public abstract record ExternalOptions : ModularPipelines.Options.CommandLineToolOptions
            {
                [ModularPipelines.Attributes.CliOption("--project")]
                public string? Project { get; set; }
            }
            """,
            """
            public sealed record FirstOptions : ExternalOptions;
            public sealed record SecondOptions : ExternalOptions;
            """,
            globalOptions: new Dictionary<string, string>
            {
                ["build_property.PublishTrimmed"] = "true",
            });
        var generated = string.Concat(result.GeneratedTrees.Select(static tree => tree.ToString()));

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(CountOccurrences(generated, "static instance => ((global::ExternalOptions)instance).@Project,"))
                .IsEqualTo(1);
            await Assert.That(CountOccurrences(generated, "CliOptionAttribute(\"--project\")")).IsEqualTo(1);
            await Assert.That(generated).Contains("GeneratedCommandMetadata.RegisterExternal(");
        }
    }

    [Test]
    public async Task Parts_Used_By_One_Registration_Stay_Inline()
    {
        var result = GeneratorTestRunner.Run(
            new CommandOptionsGenerator(),
            Infrastructure,
            """
            public sealed record FirstOptions : ModularPipelines.Options.CommandLineToolOptions
            {
                [ModularPipelines.Attributes.CliOption("--account")]
                public string? Account { get; set; }
            }

            public sealed record SecondOptions : ModularPipelines.Options.CommandLineToolOptions
            {
                [ModularPipelines.Attributes.CliOption("--account")]
                public string? Account { get; set; }
            }
            """);

        await Assert.That(result.GeneratedTrees.Select(static tree => Path.GetFileName(tree.FilePath)))
            .IsEquivalentTo(["ModularPipelines.RuntimeMetadata.g.cs"]);
    }

    private static int CountOccurrences(string source, string value)
    {
        var count = 0;
        for (var index = source.IndexOf(value, StringComparison.Ordinal);
             index >= 0;
             index = source.IndexOf(value, index + value.Length, StringComparison.Ordinal))
        {
            count++;
        }

        return count;
    }
}
