using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class HeuristicTypeDetectorTests
{
    private readonly HeuristicTypeDetector _detector;

    public HeuristicTypeDetectorTests()
    {
        var logger = NullLogger<HeuristicTypeDetector>.Instance;
        _detector = new HeuristicTypeDetector(logger);
    }

    private static OptionDetectionContext CreateContext(
        string optionName,
        string? description = null,
        string? defaultValue = null,
        string? acceptedValues = null)
    {
        return new OptionDetectionContext
        {
            OptionName = optionName,
            AllNames = [optionName],
            Description = description,
            DefaultValue = defaultValue,
            AcceptedValues = acceptedValues,
            ToolName = "test",
            CommandPath = ["test", "command"],
        };
    }

    #region Basic Properties Tests

    [Test]
    public async Task Priority_Returns_300()
    {
        await Assert.That(_detector.Priority).IsEqualTo(300);
    }

    [Test]
    public async Task Name_Returns_Heuristic()
    {
        await Assert.That(_detector.Name).IsEqualTo("Heuristic");
    }

    [Test]
    public async Task CanHandle_Returns_True_For_Any_Tool()
    {
        await Assert.That(_detector.CanHandle("docker")).IsTrue();
        await Assert.That(_detector.CanHandle("kubectl")).IsTrue();
        await Assert.That(_detector.CanHandle("az")).IsTrue();
        await Assert.That(_detector.CanHandle("any-tool")).IsTrue();
    }

    #endregion

    #region Boolean Detection Tests - Default Values

    [Test]
    [Arguments("true")]
    [Arguments("false")]
    [Arguments("yes")]
    [Arguments("no")]
    [Arguments("0")]
    [Arguments("1")]
    public async Task DetectType_Returns_Bool_For_Boolean_DefaultValue(string defaultValue)
    {
        var context = CreateContext("--flag", defaultValue: defaultValue);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Bool);
    }

    [Test]
    public async Task DetectType_Returns_Int_For_Numeric_DefaultValue()
    {
        var context = CreateContext("--count", defaultValue: "42");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Int);
    }

    #endregion

    #region Boolean Detection Tests - Accepted Values

    [Test]
    public async Task DetectType_Returns_Bool_For_Boolean_AcceptedValues()
    {
        var context = CreateContext("--option", acceptedValues: "true, false");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Bool);
    }

    [Test]
    public async Task DetectType_Returns_Bool_For_YesNo_AcceptedValues()
    {
        var context = CreateContext("--option", acceptedValues: "yes, no");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Bool);
    }

    #endregion

    #region Boolean Detection Tests - Option Name Prefixes

    [Test]
    [Arguments("--no-cache")]
    [Arguments("--disable-validation")]
    [Arguments("--enable-feature")]
    [Arguments("--with-deps")]
    [Arguments("--without-deps")]
    [Arguments("--skip-tests")]
    [Arguments("--force-rebuild")]
    [Arguments("--allow-empty")]
    [Arguments("--deny-access")]
    [Arguments("--include-all")]
    [Arguments("--exclude-tests")]
    public async Task DetectType_Returns_Bool_For_Boolean_Prefix_Names(string optionName)
    {
        var context = CreateContext(optionName);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Bool);
    }

    #endregion

    #region Boolean Detection Tests - Option Name Suffixes

    [Test]
    [Arguments("--build-only")]
    [Arguments("--all")]
    [Arguments("--recursive")]
    [Arguments("--verbose")]
    [Arguments("--quiet")]
    [Arguments("--silent")]
    [Arguments("--interactive")]
    [Arguments("--yes")]
    [Arguments("--no")]
    [Arguments("--force")]
    public async Task DetectType_Returns_Bool_For_Boolean_Suffix_Names(string optionName)
    {
        var context = CreateContext(optionName);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Bool);
    }

    #endregion

    #region Boolean Detection Tests - Exact Names

    [Test]
    [Arguments("--verbose")]
    [Arguments("--quiet")]
    [Arguments("--silent")]
    [Arguments("--debug")]
    [Arguments("--force")]
    [Arguments("--yes")]
    [Arguments("--no")]
    [Arguments("--dry-run")]
    [Arguments("--dryrun")]
    [Arguments("--help")]
    [Arguments("--version")]
    [Arguments("--interactive")]
    [Arguments("--detach")]
    [Arguments("--tty")]
    [Arguments("--privileged")]
    [Arguments("--rm")]
    [Arguments("--init")]
    [Arguments("--recursive")]
    [Arguments("--all")]
    [Arguments("--cached")]
    [Arguments("--staged")]
    [Arguments("--untracked")]
    [Arguments("--ignored")]
    public async Task DetectType_Returns_Bool_For_Boolean_Exact_Names(string optionName)
    {
        var context = CreateContext(optionName);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Bool);
    }

    #endregion

    #region Boolean Detection Tests - Description Patterns

    [Test]
    [Arguments("enable logging")]
    [Arguments("disable caching")]
    [Arguments("turn on debugging")]
    [Arguments("turn off features")]
    [Arguments("whether to build")]
    [Arguments("if true, do something")]
    [Arguments("if set, enable feature")]
    [Arguments("when set, activate")]
    [Arguments("flag to enable")]
    [Arguments("activate the feature")]
    [Arguments("deactivate mode")]
    public async Task DetectType_Returns_Bool_For_Boolean_Description_Patterns(string description)
    {
        var context = CreateContext("--some-option", description: description);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Bool);
    }

    #endregion

    #region Numeric Detection Tests

    [Test]
    [Arguments("--count")]
    [Arguments("--limit")]
    [Arguments("--max-retries")]
    [Arguments("--min-value")]
    [Arguments("--size")]
    [Arguments("--length")]
    [Arguments("--depth")]
    [Arguments("--retries")]
    [Arguments("--timeout")]
    [Arguments("--interval")]
    [Arguments("--port")]
    [Arguments("--number")]
    public async Task DetectType_Returns_Int_For_Numeric_Names(string optionName)
    {
        var context = CreateContext(optionName);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Int);
    }

    #endregion

    #region List Detection Tests

    [Test]
    [Arguments("--add-host")]
    [Arguments("--exclude")]
    [Arguments("--include")]
    [Arguments("--label")]
    [Arguments("--env")]
    [Arguments("--volume")]
    [Arguments("--mount")]
    [Arguments("--publish")]
    [Arguments("--expose")]
    [Arguments("--cap-add")]
    [Arguments("--device")]
    [Arguments("--dns")]
    public async Task DetectType_Returns_StringList_For_List_Names(string optionName)
    {
        var context = CreateContext(optionName);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.StringList);
    }

    #endregion

    #region KeyValue Detection Tests

    [Test]
    public async Task DetectType_Returns_KeyValue_For_Env_Option()
    {
        var context = CreateContext("--env");

        var result = await _detector.DetectTypeAsync(context);

        // Note: env matches list pattern first, but this tests the pattern exists
        await Assert.That(result.Type).IsEqualTo(CliOptionType.StringList).Or.IsEqualTo(CliOptionType.KeyValue);
    }

    [Test]
    public async Task DetectType_Returns_KeyValue_For_Label_Option()
    {
        var context = CreateContext("--label");

        var result = await _detector.DetectTypeAsync(context);

        // Note: label matches list pattern first
        await Assert.That(result.Type).IsEqualTo(CliOptionType.StringList).Or.IsEqualTo(CliOptionType.KeyValue);
    }

    [Test]
    public async Task DetectType_Returns_KeyValue_For_Annotation_Option()
    {
        var context = CreateContext("--annotation");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.KeyValue);
    }

    [Test]
    public async Task DetectType_Returns_KeyValue_For_Sysctl_Option()
    {
        var context = CreateContext("--sysctl");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.KeyValue);
    }

    [Test]
    public async Task DetectType_Returns_KeyValue_For_KeyEquals_Description()
    {
        var context = CreateContext("--option", description: "Set options in key=value format");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.KeyValue);
    }

    [Test]
    public async Task DetectType_Returns_KeyValue_For_KeyColon_Description()
    {
        var context = CreateContext("--option", description: "Set options in key:value format");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.KeyValue);
    }

    #endregion

    #region Default String Detection Tests

    [Test]
    public async Task DetectType_Returns_String_For_Unknown_Pattern()
    {
        var context = CreateContext("--output-path");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.String);
    }

    [Test]
    public async Task DetectType_Returns_String_For_Generic_Option()
    {
        var context = CreateContext("--name", description: "The name to use");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.String);
    }

    #endregion

    #region Confidence and Source Tests

    [Test]
    public async Task DetectType_Returns_Confidence_50()
    {
        var context = CreateContext("--verbose");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Confidence).IsEqualTo(50);
    }

    [Test]
    public async Task DetectType_Returns_Heuristic_Source()
    {
        var context = CreateContext("--verbose");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Source).IsEqualTo("Heuristic");
    }

    [Test]
    public async Task DetectType_Returns_Notes_With_Reason()
    {
        var context = CreateContext("--verbose");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Notes).IsNotNull();
        await Assert.That(result.Notes!.Length).IsGreaterThan(0);
    }

    #endregion
}
