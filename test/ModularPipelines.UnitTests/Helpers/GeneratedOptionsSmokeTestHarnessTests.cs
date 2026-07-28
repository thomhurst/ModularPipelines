using ModularPipelines.Attributes;
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
        public CliOptionValuePair? Pair { get; init; }
    }
}
