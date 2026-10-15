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
        var sharedSource = sources["ModularPipelines.RuntimeMetadata.SharedParts.g.cs"];
        var registrationSource = sources["ModularPipelines.RuntimeMetadata.g.cs"];
        var allSources = string.Concat(sources.Values);

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics).IsEmpty();
            await Assert.That(sharedSource).Contains("PropertyCommandLinePart Command0000 =");
            await Assert.That(sharedSource).Contains("SecretPropertyAccessor Secret0000 =");
            await Assert.That(sharedSource).DoesNotContain("Command0001");
            await Assert.That(CountOccurrences(allSources, "((global::WideOptions)instance).@Account")).IsEqualTo(1);
            await Assert.That(CountOccurrences(allSources, "((global::WideOptions)instance).@Token")).IsEqualTo(1);
            // The abstract base registers too, so all three registrations reference the shared parts.
            await Assert.That(CountOccurrences(registrationSource, "RuntimeMetadataSharedParts.Command0000,")).IsEqualTo(3);
            await Assert.That(CountOccurrences(registrationSource, "RuntimeMetadataSharedParts.Secret0000,")).IsEqualTo(3);
            await Assert.That(registrationSource).Contains("((global::FirstOptions)instance).@First");
            await Assert.That(registrationSource).Contains("((global::SecondOptions)instance).@Second");
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
