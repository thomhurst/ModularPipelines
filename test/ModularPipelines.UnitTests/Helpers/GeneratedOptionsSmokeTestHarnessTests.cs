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
        ];

        await Assert.That(arguments.SequenceEqual(expected, StringComparer.Ordinal)).IsTrue();
    }

    [Test]
    public async Task Reflection_Model_Uses_Most_Derived_Shadowed_Property()
    {
        var result = GeneratedOptionsSmokeTestHarness.ValidateOptionsTypeUsingReflection(
            typeof(ShadowedOptions));

        await Assert.That(result.PropertiesTested).IsEqualTo(1);
    }

    [Test]
    public async Task Reflection_Model_Ignores_Setter_Only_Shadow()
    {
        var result = GeneratedOptionsSmokeTestHarness.ValidateOptionsTypeUsingReflection(
            typeof(SetterOnlyShadowedOptions));

        await Assert.That(result.PropertiesTested).IsEqualTo(1);
    }

    private sealed record RepresentativeOptions : CommandLineToolOptions
    {
        [CliArgument(0, PrependOptionTerminator = true)]
        public string? Target { get; init; }

        [CliArgument(1, Name = "inline")]
        public string? InlineArgument { get; init; }

        [CliFlag("--verbose", ShortForm = "-v", PreferShortForm = true)]
        public bool? Verbose { get; init; }

        [CliOption("--output", Format = OptionFormat.EqualsSeparated)]
        public string? Output { get; init; }

        [CliOption("--pair")]
        public CliValuePair? Pair { get; init; }
    }

    private record ShadowedOptionsBase : CommandLineToolOptions
    {
        [CliOption("--progress")]
        public string? Progress { get; init; }
    }

    private sealed record ShadowedOptions : ShadowedOptionsBase
    {
        [CliOption("--progress", Format = OptionFormat.EqualsSeparated)]
        public new int? Progress { get; init; }
    }

    private record SetterOnlyShadowedOptionsBase : CommandLineToolOptions
    {
        [CliOption("--value")]
        public string? Value { get; init; }
    }

    private sealed record SetterOnlyShadowedOptions : SetterOnlyShadowedOptionsBase
    {
        public new string? Value { set { } }
    }
}
