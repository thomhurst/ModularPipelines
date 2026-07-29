using Microsoft.CodeAnalysis.Diagnostics;

namespace ModularPipelines.Development.Analyzers.UnitTests;

public class AnalyzerMetadataTests
{
    private const string DocumentationBaseUrl =
        "https://thomhurst.github.io/ModularPipelines/docs/next/analyzers/";

    [Test]
    public async Task DevelopmentRulesContinueTheUnifiedIdFamily()
    {
        DiagnosticAnalyzer[] analyzers =
        [
            new VirtualSwitchPropertyAnalyzer(),
            new VirtualCommandAnalyzer(),
        ];

        var rules = analyzers.SelectMany(analyzer => analyzer.SupportedDiagnostics).ToArray();

        await Assert.That(rules.Select(rule => rule.Id)).IsEquivalentTo(["MP0011", "MP0012"]);

        foreach (var rule in rules)
        {
            await Assert.That(rule.HelpLinkUri).IsEqualTo(DocumentationBaseUrl + rule.Id);
        }
    }
}
