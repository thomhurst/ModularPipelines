using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.NonGenericModuleResultAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class NonGenericModuleResultAnalyzerTests
{
    private const string Header = """
        using System;
        using System.Linq;
        using System.Collections.Generic;
        using System.Threading;
        using System.Threading.Tasks;
        using ModularPipelines;
        using ModularPipelines.Enums;
        using ModularPipelines.Events;
        using ModularPipelines.Context;
        using FailureAlias = ModularPipelines.ModuleResult.Failure;
        """;

    [TestMethod]
    public void Rule_Is_Warning() =>
        Assert.AreEqual(DiagnosticSeverity.Warning, NonGenericModuleResultAnalyzer.Rule.DefaultSeverity);

    [TestMethod]
    public async Task Reports_Summary_OfType_Filters()
    {
        await VerifyCS.VerifyAnalyzerAsync($$"""
            {{Header}}
            public class Consumer
            {
                public void Inspect(PipelineSummary summary)
                {
                    _ = summary.Results.OfType<{|#0:ModuleResult.Failure|}>();
                    _ = Enumerable.OfType<{|#1:ModuleResult.Skipped|}>(summary.Results);
                    _ = summary.Results.Where(result => result.Name != "test").OfType<{|#2:FailureAlias|}>();
                    _ = summary.Results.AsQueryable().OfType<{|#3:ModuleResult.Skipped|}>();
                    _ = summary.Results?.OfType<{|#4:ModuleResult.Failure|}>();
                }
            }
            """, Expected(0), Expected(1), Expected(2), Expected(3), Expected(4));
    }

    [TestMethod]
    public async Task Reports_End_Handler_Patterns()
    {
        await VerifyCS.VerifyAnalyzerAsync($$"""
            {{Header}}
            public class Handler : IModuleEndHandler
            {
                public Task OnModuleEndAsync(IModuleHookContext context, IModuleResult result, CancellationToken cancellationToken)
                {
                    _ = result is {|#0:ModuleResult.Failure|};
                    _ = result is {|#1:ModuleResult.Skipped|} skipped;
                    _ = result is not {|#2:FailureAlias|};
                    _ = result is {|#3:ModuleResult.Failure|} { Exception: InvalidOperationException };
                    _ = result switch
                    {
                        {|#4:ModuleResult.Skipped|}(var decision) => decision.Reason,
                        _ => "other"
                    };
                    switch (result)
                    {
                        case {|#5:ModuleResult.Failure|} failure:
                            break;
                    }
                    return Task.CompletedTask;
                }
            }
            """, Expected(0), Expected(1), Expected(2), Expected(3), Expected(4), Expected(5));
    }

    [TestMethod]
    public async Task Reports_Base_Result_And_Compound_Patterns()
    {
        await VerifyCS.VerifyAnalyzerAsync($$"""
            {{Header}}
            public class Consumer
            {
                public void Inspect(ModuleResult result, IEnumerable<ModuleResult> results)
                {
                    _ = result is {|#0:ModuleResult.Failure|} or {|#1:ModuleResult.Skipped|};
                    _ = results.OfType<{|#2:ModuleResult.Failure|}>();
                }
            }
            """, Expected(0), Expected(1), Expected(2));
    }

    [TestMethod]
    public async Task Allows_Generic_Variants_And_Safe_Accessors()
    {
        await VerifyCS.VerifyAnalyzerAsync($$"""
            {{Header}}
            public class Consumer
            {
                public void Inspect(IModuleResult result, PipelineSummary summary, ModuleResult<int> typed,
                    IEnumerable<ModuleResult<int>> typedResults)
                {
                    _ = result is ModuleResult<int>.Failure;
                    _ = result is ModuleResult<int>.Skipped skipped;
                    _ = typed is ModuleResult<int>.Failure { Exception: InvalidOperationException };
                    _ = summary.Results.OfType<ModuleResult<int>.Failure>();
                    _ = typedResults.OfType<ModuleResult.Failure>();
                    _ = result.Status == ModuleStatus.Failed;
                    _ = result.ExceptionOrDefault is not null;
                    _ = result.SkipDecisionOrDefault is not null;
                }
            }
            """);
    }

    [TestMethod]
    public async Task Allows_NonGeneric_Construction_Conversion_And_Known_NonGeneric_Values()
    {
        await VerifyCS.VerifyAnalyzerAsync($$"""
            {{Header}}
            public class Consumer
            {
                public void Inspect()
                {
                    var failure = new ModuleResult.Failure(new Exception())
                    {
                        Name = "test", Duration = TimeSpan.Zero, StartTime = DateTimeOffset.Now,
                        EndTime = DateTimeOffset.Now, Status = ModuleStatus.Failed
                    };
                    var skipped = new ModuleResult.Skipped(SkipDecision.Skip("reason"))
                    {
                        Name = "test", Duration = TimeSpan.Zero, StartTime = DateTimeOffset.Now,
                        EndTime = DateTimeOffset.Now, Status = ModuleStatus.Skipped
                    };
                    ModuleResult<int> convertedFailure = failure;
                    ModuleResult<int> convertedSkipped = skipped;
                    _ = failure is ModuleResult.Failure;
                    _ = skipped is ModuleResult.Skipped;
                    _ = new[] { failure }.OfType<ModuleResult.Failure>();
                }
            }
            """);
    }

    [TestMethod]
    public async Task Ignores_Unrelated_Types_Methods_And_Object_Collections()
    {
        await VerifyCS.VerifyAnalyzerAsync($$"""
            {{Header}}
            public class Consumer
            {
                public void Inspect(IModuleResult result, PipelineSummary summary, object value, object[] objects)
                {
                    _ = result is Other.ModuleResult.Failure;
                    _ = value is ModuleResult.Failure;
                    _ = objects.OfType<ModuleResult.Skipped>();
                    _ = Other.Filters.OfType<ModuleResult.Failure>(summary.Results);
                }
            }
            namespace Other
            {
                public class ModuleResult
                {
                    public interface Failure { }
                }
                public static class Filters
                {
                    public static T OfType<T>(IEnumerable<IModuleResult> source) => default(T);
                }
            }
            """);
    }

    private static Microsoft.CodeAnalysis.Testing.DiagnosticResult Expected(int location) =>
        VerifyCS.Diagnostic().WithLocation(location);
}
