using ModularPipelines.Attributes;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class GeneratedOptionsSmokeTestHarnessTests
{
    [Test]
    public async Task Validates_Each_Options_Property_Independently()
    {
        var result = GeneratedOptionsSmokeTestHarness.ValidateOptionsType(typeof(RepresentativeOptions));

        await Assert.That(result.OptionsTypesTested).IsEqualTo(1);
        await Assert.That(result.PropertiesTested).IsEqualTo(5);
    }

    [Test]
    public async Task Representative_Options_Render_Known_Arguments_In_Order()
    {
        var model = new CommandModelProvider().GetCommandModel(typeof(RepresentativeOptions));
        var arguments = new CommandArgumentBuilder().BuildArguments(
            model,
            new RepresentativeOptions
            {
                Target = "target-value",
                InlineArgument = "inline-value",
                Verbose = true,
                Output = "output-value",
                Pair = new CliValuePair("first", "second"),
            });

        string[] expected =
        [
            "-v",
            "--output=output-value",
            "--pair",
            "first",
            "second",
            "--",
            "target-value",
            "inline-value",
        ];

        await Assert.That(arguments.SequenceEqual(expected, StringComparer.Ordinal)).IsTrue();
    }

    internal sealed record RepresentativeOptions : CommandLineToolOptions
    {
        [CliArgument(0, PrependOptionTerminator = true, Required = true)]
        public string? Target { get; init; }

        [CliArgument(1)]
        public string? InlineArgument { get; init; }

        [CliFlag("--verbose", ShortForm = "-v", PreferShortForm = true)]
        public bool? Verbose { get; init; }

        [CliOption("--output", Format = OptionFormat.EqualsSeparated)]
        public string? Output { get; init; }

        [CliOption("--pair")]
        public CliValuePair? Pair { get; init; }
    }
}
