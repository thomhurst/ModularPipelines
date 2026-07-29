using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class AnalyzerMetadataTests
{
    private const string DocumentationBaseUrl =
        "https://thomhurst.github.io/ModularPipelines/docs/next/analyzers/";

    [TestMethod]
    public void PublicRulesUseUnifiedIdsAndHelpLinks()
    {
        var rules = new[]
        {
            MissingDependsOnAttributeAnalyzer.Rule,
            EnumerableModuleResultAnalyzer.Rule,
            LoggerInConstructorAnalyzer.Rule,
            ConsoleUseAnalyzer.Rule,
            ConflictingDependsOnAttributeAnalyzer.Rule,
            AsyncModuleAnalyzer.Rule,
            AwaitThisAnalyzer.Rule,
            StatefulModuleAnalyzer.Rule,
            InvalidDependsOnTypeAnalyzer.Rule,
            SelfDependencyAnalyzer.Rule,
            ModuleRegistrationAnalyzer.UnregisteredModuleRule,
            ModuleAsyncSafetyAnalyzer.AsyncVoidRule,
            ModuleAsyncSafetyAnalyzer.BlockingCallRule,
            ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenRule,
            ModuleAsyncSafetyAnalyzer.ThreadSleepRule,
            ModuleRegistrationAnalyzer.NonPublicModuleRule,
            DuplicateDependsOnAnalyzer.Rule,
        };

        Assert.AreSequenceEqual(
            Enumerable.Range(1, 10)
                .Concat(Enumerable.Range(13, 7))
                .Select(index => $"MP{index:0000}"),
            rules.Select(rule => rule.Id));

        foreach (var rule in rules)
        {
            Assert.AreEqual(DocumentationBaseUrl + rule.Id, rule.HelpLinkUri);
            Assert.IsFalse(string.IsNullOrWhiteSpace(rule.Description.ToString()));
        }
    }
}
