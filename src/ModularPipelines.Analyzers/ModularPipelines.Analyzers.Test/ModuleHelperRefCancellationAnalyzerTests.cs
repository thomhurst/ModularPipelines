using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleHelperRefCancellationAnalyzerTests
{
    [TestMethod]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var saved = first; var local = first; first = ref local; first = saved; return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var saved = first; var local = first; first = ref local; saved = default; first = saved; return first;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var saved = first; var local = first; first = ref local; first = saved; local = default; return first;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var saved = first; var local = first; first = ref local; ref CancellationToken alias = ref first; alias = saved; return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var saved = first; var local = first; first = ref local; int count = 0; (first, count) = (saved, 1); return first;", false)]
    [DataRow("ref other, ref token", "CancellationToken other = default;", "first = ref second; second = ref first; return second;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "second = ref first; first = ref second; return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "first = ref second; second = ref first; return second;", true)]
    [DataRow("ref other, ref token", "CancellationToken other = default;", "first = ref second; second = ref first; first = default; return second;", true)]
    [DataRow("ref other, ref token", "CancellationToken other = default;", "first = ref second; second = ref first; first = ref second; second = ref first; return second;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "CancellationToken unrelated = default; static void Clear(ref CancellationToken value) { value = default; } Clear(ref unrelated); return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var intact = first; var local = first; second = ref local; void Refresh() { local = intact; } Refresh(); return second;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; var saved = local; second = ref local; void Copy() { local = saved; } Copy(); return second;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; var saved = local; second = ref local; saved = default; void Copy() { local = saved; } Copy(); return second;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; var saved = local; second = ref local; ref CancellationToken alias = ref saved; alias = default; void Copy() { local = saved; } Copy(); return second;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; var saved = local; second = ref local; void Copy() { saved = default; local = saved; } Copy(); return second;", true)]
    [DataRow("ref token, ref token", "", "var local = first; second = default; var intact = first; first = ref local; void Copy() { local = intact; } Copy(); return first;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; var saved = local; var copy = saved; second = ref local; System.Action refresh = () => local = copy; refresh(); return second;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; void Clear() { if (Flag) Clear(); local = default; } Clear(); return second;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; void Clear() { local = default; } void Unused() { Clear(); } return second;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; void Clear() { local = default; } Clear(); return second;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; void Clear() { local = default; } return second;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; System.Action clear = () => local = default; clear(); return second;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; void Clear() { local = default; } void Invoke() { Clear(); } Invoke(); return second;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; Clear(); return second; void Clear() { local = default; }", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; void Clear() { local = default; } var saved = second; Clear(); return saved;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; void Clear() { local = default; } Clear(); local = first; return second;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; void Clear() { local = default; } if (false) Clear(); return second;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; CancellationToken unrelated = default; void Clear() { unrelated = default; } Clear(); return second;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; goto Clear; Read: return second; Clear: second = default; goto Read;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; goto Clear; Read: return second; Clear: second = local; goto Read;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var saved = first; if (Flag) { first = ref second; return saved; } return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var saved = first; if (Flag) { first = ref second; } return first;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var saved = first; if (Flag) { first = ref second; throw new System.Exception(); } return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; int count = 0; (count, (local, count)) = (1, (local, 2)); return second;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; int count = 0; (count, (local, count)) = (1, (default(CancellationToken), 2)); return second;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; CancellationToken empty = default; (local, empty) = (empty, local); return second;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; int count = 0; (local, count) = (local, 1); return second;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; int count = 0; (second, count) = (local, 1); return second;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; int count = 0; (local, count) = (default(CancellationToken), 1); return second;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; CancellationToken other = default; (local, other) = (local, default(CancellationToken)); return second;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; (local, second) = (local, default(CancellationToken)); return second;", true)]
    [DataRow("ref token, ref token", "", "second = default; return first;", true)]
    [DataRow("ref token, ref token", "", "first = default; return second;", true)]
    [DataRow("second: ref token, first: ref token", "", "second = default; return first;", true)]
    [DataRow("ref token, ref alias", "ref CancellationToken alias = ref token;", "second = default; return first;", true)]
    [DataRow("ref alias, ref token", "ref CancellationToken alias = ref token;", "second = default; return first;", true)]
    [DataRow("ref token, ref alias", "ref CancellationToken alias = ref token; ref CancellationToken chained = ref alias; alias = ref chained;", "second = default; return first;", true)]
    [DataRow("ref token, ref token", "", "return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "second = default; return first;", false)]
    [DataRow("second: ref other, first: ref token", "CancellationToken other = default;", "second = default; return first;", false)]
    [DataRow("ref token, ref alias", "CancellationToken other = default; ref CancellationToken alias = ref other;", "second = default; return first;", false)]
    [DataRow("ref token, ref token", "", "var original = first; second = default; return original;", false)]
    [DataRow("ref token, ref token", "", "if (false) second = default; return first;", false)]
    [DataRow("ref token, ref alias", "CancellationToken other = default; ref CancellationToken alias = ref token; alias = ref other;", "second = default; return first;", false)]
    [DataRow("ref token, ref alias", "CancellationToken other = default; ref CancellationToken alias = ref other; alias = ref token;", "second = default; return first;", true)]
    [DataRow("ref token, ref token", "", "second = first; return first;", false)]
    [DataRow("ref token, ref token", "", "first = second; return second;", false)]
    [DataRow("ref token, ref token", "", "second = ref first; return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = saved; return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; saved = default; second = saved; return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; second = saved; return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; var copy = saved; second = copy; return first;", false)]
    [DataRow("ref token, ref token", "", "second = Forward(ref first, ref second); return first;", true)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref second; return first;", false)]
    [DataRow("ref token, ref token", "", "var stored = first; ref CancellationToken alias = ref stored; alias = default; second = stored; return first;", true)]
    [DataRow("ref token, ref token", "", "var stored = first; ref CancellationToken alias = ref stored; second = stored; return first;", false)]
    [DataRow("ref token, ref token", "", "var stored = first; ref CancellationToken alias = ref stored; alias = default; stored = first; second = stored; return first;", false)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref second; alias = default; return first;", true)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref first; alias = ref second; alias = default; return first;", true)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref second; var saved = first; alias = default; return saved;", false)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref second; alias = default; var saved = first; return saved;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "second = ref first; second = default; return first;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "second = ref first; return first;", false)]
    [DataRow("ref token, ref token", "", "static void Clear(ref CancellationToken x) { x = default; } return first;", false)]
    [DataRow("ref token, ref token", "", "static void Clear(ref CancellationToken x) { x = default; } Clear(ref second); return first;", true)]
    [DataRow("ref token, ref token", "", "int count = 0; ref int alias = ref count; alias = 0; return first;", false)]
    [DataRow("ref token, ref token", "", "ref int count = ref Count; count = 0; return first;", false)]
    [DataRow("ref token, ref token", "", "if (Flag) { var saved = first; second = default; return saved; } return first;", false)]
    [DataRow("ref token, ref token", "", "if (Flag) { second = default; return first; } return first;", true)]
    [DataRow("ref token, ref token", "", "if (Flag) { second = default; } return first;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "first = ref second; return first;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = cancellationToken;", "first = ref second; return first;", false)]
    [DataRow("ref other, ref token", "CancellationToken other = default;", "first = ref second; return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "first = ref second; var saved = first; return saved;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var saved = first; first = ref second; return saved;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "first = ref second; first = ref second; return first;", true)]
    [DataRow("ref token, ref token", "", "try { second = default; throw new System.InvalidOperationException(); } catch { return first; }", true)]
    [DataRow("ref token, ref token", "", "try { second = default; throw new System.InvalidOperationException(); } catch { } return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; if (Flag) return first; second = default; return saved;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; void Unused() { ref CancellationToken alias = ref saved; alias = default; } second = saved; return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "first = ref second; second = default; return first;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = cancellationToken;", "first = ref second; second = default; return first;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "CancellationToken local = first; first = ref local; return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "CancellationToken local = first; first = ref local; first = default; return first;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "CancellationToken local = first; first = ref local; ref CancellationToken alias = ref local; alias = default; return first;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "CancellationToken local = first; first = ref local; first = local; return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "first = default; CancellationToken local = first; second = ref local; return second;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "CancellationToken local = first; first = default; second = ref local; return second;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "CancellationToken local = first; second = ref local; return second;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "first = default; CancellationToken local = first; CancellationToken copy = local; second = ref copy; return second;", true)]
    [DataRow("ref other, ref token", "CancellationToken other = default;", "first = ref second; ref CancellationToken alias = ref first; second = default; return alias;", true)]
    [DataRow("ref other, ref token", "CancellationToken other = default;", "first = ref second; ref CancellationToken alias = ref first; var saved = alias; second = default; return saved;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; (second, first) = (default(CancellationToken), default(CancellationToken)); return second;", true)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "var local = first; second = ref local; CancellationToken unrelated = default; (unrelated, first) = (default(CancellationToken), default(CancellationToken)); return second;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "int count = 0; (first, count) = (first, 1); return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "(first, second) = (second, first); return first;", true)]
    public async Task Checks_Helper_Ref_Argument_Storage(
        string arguments, string setup, string body, bool reports)
    {
        var call = "Task.Delay(1, snapshot)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public class BuildModule : Module<List<string>>
            {
                private static int Count;
                private static bool Flag;

                private static CancellationToken Forward(scoped ref CancellationToken first, scoped ref CancellationToken second)
                {
                    {{body}}
                }

                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context, CancellationToken cancellationToken)
                {
                    var token = cancellationToken;
                    {{setup}}
                    var snapshot = Forward({{arguments}});
                    for (var i = 0; i < 2; i++)
                        await {{(reports ? "{|#0:" + call + "|}" : call)}};
                    return null!;
                }
            }
            """;

        if (reports)
        {
            await VerifyCS.VerifyAnalyzerAsync(source,
                VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                    .WithLocation(0).WithArguments("Delay"));
        }
        else
        {
            await VerifyCS.VerifyAnalyzerAsync(source);
        }
    }

    [TestMethod]
    [DataRow(true, "(third, count) = (default(CancellationToken), 1);", true, false)]
    [DataRow(true, "(count, (third, count)) = (1, (default(CancellationToken), 2));", true, false)]
    [DataRow(true, "ref CancellationToken alias = ref third; (alias, count) = (default(CancellationToken), 1);", true, false)]
    [DataRow(true, "(third, count) = (first, 1);", false, false)]
    [DataRow(true, "(third, count) = (local, 1);", false, false)]
    [DataRow(true, "local = default; (third, count) = (local, 1);", true, false)]
    [DataRow(true, "(third, local) = (local, default(CancellationToken));", true, false)]
    [DataRow(true, "(third, local) = (default(CancellationToken), local);", true, false)]
    [DataRow(true, "local = default; (third, local) = (local, third);", true, false)]
    [DataRow(true, "CancellationToken unrelated = default; scoped ref CancellationToken alias = ref third; alias = ref unrelated; (alias, count) = (default(CancellationToken), 1);", false, false)]
    [DataRow(true, "(third, count) = (local, 1); third = default;", true, false)]
    [DataRow(true, "intact = default; Copy(); (third, count) = (local, 1);", true, false)]
    [DataRow(true, "CancellationToken unrelated = default; (unrelated, count) = (default(CancellationToken), 1);", false, false)]
    [DataRow(false, "(third, count) = (default(CancellationToken), 1);", false, false)]
    [DataRow(true, "(third, count) = (default(CancellationToken), 1);", false, true)]
    [DataRow(true, "(count, (third, count)) = (1, (default(CancellationToken), 2));", false, true)]
    [DataRow(true, "ref CancellationToken alias = ref third; (alias, count) = (default(CancellationToken), 1);", false, true)]
    [DataRow(true, "(third, local) = (default(CancellationToken), local);", false, true)]
    [DataRow(true, "(third, count) = (default(CancellationToken), 1); first = default;", true, true)]
    [DataRow(true, "third = ref first; (third, count) = (default(CancellationToken), 1);", true, true)]
    public async Task Checks_Tuple_Ref_Parameter_Snapshot(bool thirdByRef, string store, bool reports, bool distinctThird)
    {
        var call = "Task.Delay(1, snapshot)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public class BuildModule : Module<List<string>>
            {
                private static CancellationToken Forward(
                    ref CancellationToken first, scoped ref CancellationToken second,
                    {{(thirdByRef ? "ref " : "")}}CancellationToken third)
                {
                    var local = first;
                    second = ref local;
                    int count = 0;
                    for (int i = 0; i < 2; i++)
                    {
                        var intact = first;
                        void Copy() { local = intact; }
                        Copy();
                        {{store}}
                    }
                    return second;
                }

                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context, CancellationToken cancellationToken)
                {
                    var token = cancellationToken;
                    CancellationToken other = default;
                    CancellationToken unrelated = default;
                    var snapshot = Forward(ref token, ref other, {{(thirdByRef ? "ref " : "")}}{{(distinctThird ? "unrelated" : "token")}});
                    for (var i = 0; i < 2; i++)
                        await {{(reports ? "{|#0:" + call + "|}" : call)}};
                    return null!;
                }
            }
            """;

        if (reports)
        {
            await VerifyCS.VerifyAnalyzerAsync(source,
                VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                    .WithLocation(0).WithArguments("Delay"));
        }
        else
        {
            await VerifyCS.VerifyAnalyzerAsync(source);
        }
    }

    [TestMethod]
    [DataRow("second = ref third; second = default; return first;", false)]
    [DataRow("second = ref third; second = default; var saved = first; return saved;", false)]
    [DataRow("third = ref first; third = default; return first;", true)]
    [DataRow("ref CancellationToken alias = ref second; alias = ref third; var saved = first; alias = default; return saved;", false)]
    [DataRow("second = ref third; second = ref first; second = default; return first;", true)]
    [DataRow("second = ref first; second = ref third; second = default; return first;", false)]
    [DataRow("ref CancellationToken alias = ref second; var saved = first; Clear(ref alias); return saved;", false)]
    [DataRow("ref CancellationToken alias = ref second; Clear(ref alias); return first;", true)]
    [DataRow("bool done = false; Again: second = ref first; var saved = second; second = ref third; if (!done) { done = true; goto Again; } return saved;", false)]
    [DataRow("bool done = false; Again: if (!done) second = ref first; var saved = second; second = ref third; if (!done) { done = true; goto Again; } return saved;", true)]
    public async Task Checks_Helper_Ref_Parameter_Rebinding(string body, bool reports)
    {
        var call = "Task.Delay(1, snapshot)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public class BuildModule : Module<List<string>>
            {
                private static void Clear(ref CancellationToken value) => value = default;

                private static CancellationToken Forward(
                    ref CancellationToken first, ref CancellationToken second, ref CancellationToken third)
                {
                    {{body}}
                }

                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context, CancellationToken cancellationToken)
                {
                    var token = cancellationToken;
                    CancellationToken other = default;
                    var snapshot = Forward(ref token, ref token, ref other);
                    for (var i = 0; i < 2; i++)
                        await {{(reports ? "{|#0:" + call + "|}" : call)}};
                    return null!;
                }
            }
            """;

        if (reports)
        {
            await VerifyCS.VerifyAnalyzerAsync(source,
                VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                    .WithLocation(0).WithArguments("Delay"));
        }
        else
        {
            await VerifyCS.VerifyAnalyzerAsync(source);
        }
    }

    [TestMethod]
    [DataRow("second = ref first; second = default; return second;", true)]
    [DataRow("second = ref first; return second;", false)]
    [DataRow("second = ref first; var saved = second; second = default; return saved;", false)]
    [DataRow("first = default; second = ref first; return second;", true)]
    [DataRow("second = default; return first;", false)]
    [DataRow("first = default; return first;", true)]
    [DataRow("second = ref first; void Clear() { first = default; } Clear(); return second;", true)]
    [DataRow("second = ref first; void Clear() { first = default; } return second;", false)]
    [DataRow("second = ref first; var saved = first; void Copy() { first = saved; } Copy(); return second;", false)]
    [DataRow("second = ref first; var saved = first; saved = default; void Copy() { first = saved; } Copy(); return second;", true)]
    [DataRow("second = ref first; void Clear() { first = default; } var saved = second; Clear(); return saved;", false)]
    [DataRow("second = ref first; System.Action clear = () => first = default; clear(); return second;", true)]
    [DataRow("second = ref first; var saved = first; int count = 0; void Copy() { (first, count) = (saved, 1); } Copy(); return second;", false)]
    [DataRow("second = ref first; var saved = first; int count = 0; void Copy() { (count, (first, count)) = (1, (saved, 2)); } Copy(); return second;", false)]
    [DataRow("second = ref first; var saved = first; saved = default; int count = 0; void Copy() { (first, count) = (saved, 1); } Copy(); return second;", true)]
    [DataRow("var saved = first; var local = first; second = ref local; for (int i = 0; i < 2; i++) { var intact = first; void Copy() { local = intact; } Copy(); first = saved; } return second;", false)]
    [DataRow("var saved = first; var local = first; second = ref local; for (int i = 0; i < 2; i++) { var intact = first; void Copy() { local = intact; } Copy(); saved = default; first = saved; } return second;", true)]
    [DataRow("var local = first; second = ref local; for (int i = 0; i < 2; i++) { var intact = first; void Copy() { local = intact; } Copy(); first = default; } return second;", true)]
    [DataRow("var local = first; second = ref local; for (int i = 0; i < 2; i++) { var intact = first; void Copy() { local = intact; } Copy(); } return second;", false)]
    [DataRow("var local = first; second = ref local; for (int i = 0; i < 2; i++) { var intact = first; void Copy() { local = intact; } Copy(); first = default; break; } return second;", false)]
    [DataRow("second = ref first; var saved = first; int count = 0; void Copy() { (first, count) = (default(CancellationToken), 1); } Copy(); return second;", true)]
    [DataRow("var local = first; var saved = local; second = ref local; int count = 0; void Copy() { (local, count) = (saved, 1); } Copy(); return second;", false)]
    [DataRow("var local = first; var saved = local; second = ref local; int count = 0; void Copy() { (local, count) = (default(CancellationToken), 1); } Copy(); return second;", true)]
    [DataRow("var saved = first; int count = 0; void Copy() { (first, count) = (saved, 1); } Copy(); return first;", false)]
    [DataRow("var saved = first; saved = default; var local = first; second = ref local; for (int i = 0; i < 2; i++) { var intact = first; void Copy() { local = intact; } Copy(); first = saved; } return second;", true)]
    [DataRow("var saved = first; ref CancellationToken alias = ref saved; alias = default; var local = first; second = ref local; for (int i = 0; i < 2; i++) { var intact = first; void Copy() { local = intact; } Copy(); first = saved; } return second;", true)]
    [DataRow("var saved = first; var local = first; second = ref local; for (int i = 0; i < 2; i++) { var intact = first; void Copy() { local = intact; } Copy(); void Restore() { first = saved; } Restore(); } return second;", false)]
    [DataRow("var saved = first; var local = first; second = ref local; for (int i = 0; i < 2; i++) { var intact = first; void Copy() { local = intact; } Copy(); first = saved; saved = default; } return second;", true)]
    [DataRow("var saved = first; var local = first; second = ref local; int count = 0; for (int i = 0; i < 2; i++) { var intact = first; void Copy() { local = intact; } Copy(); (first, count) = (saved, 1); } return second;", false)]
    public async Task Checks_Helper_Value_Parameter_Storage(string body, bool reports)
    {
        var call = "Task.Delay(1, snapshot)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public class BuildModule : Module<List<string>>
            {
                private static CancellationToken Forward(CancellationToken first, scoped ref CancellationToken second)
                {
                    {{body}}
                }

                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context, CancellationToken cancellationToken)
                {
                    CancellationToken other = default;
                    var snapshot = Forward(cancellationToken, ref other);
                    for (var i = 0; i < 2; i++)
                        await {{(reports ? "{|#0:" + call + "|}" : call)}};
                    return null!;
                }
            }
            """;

        if (reports)
        {
            await VerifyCS.VerifyAnalyzerAsync(source,
                VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                    .WithLocation(0).WithArguments("Delay"));
        }
        else
        {
            await VerifyCS.VerifyAnalyzerAsync(source);
        }
    }

    [TestMethod]
    [DataRow("for (var i = 0; i < 2; i++)", true, "")]
    [DataRow("for (var i = 0; i < 2; i++)", false, "alias = ref other;")]
    [DataRow("while (context != null)", false, "alias = ref other;")]
    [DataRow("while (context != null)", true, "")]
    [DataRow("for (var i = 0; i < 2; i++)", true, "if (i > 0) goto Call; alias = ref other; Call:")]
    [DataRow("if (context != null)", false, "")]
    [DataRow("for (var i = 0; i < 2; i++)", false, "alias = ref other; if (i > 0) goto Call; Call:")]
    [DataRow("for (var i = 0; i < 2; i++)", true, "if (i == 0) alias = ref other;")]
    public async Task Checks_Ref_Rebinding_After_Helper_Call(string control, bool reports, string reset)
    {
        var call = "Task.Delay(1, snapshot)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public class BuildModule : Module<List<string>>
            {
                private static int Count;
                private static bool Flag;

                private static CancellationToken Forward(ref CancellationToken first, ref CancellationToken second)
                {
                    second = default;
                    return first;
                }

                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context, CancellationToken cancellationToken)
                {
                    var token = cancellationToken;
                    var other = cancellationToken;
                    ref CancellationToken alias = ref other;
                    var snapshot = token;
                    {{control}}
                    {
                        {{reset}}
                        snapshot = Forward(ref token, ref alias);
                        alias = ref token;
                    }

                    for (var i = 0; i < 2; i++)
                        await {{(reports ? "{|#0:" + call + "|}" : call)}};
                    return null!;
                }
            }
            """;

        if (reports)
        {
            await VerifyCS.VerifyAnalyzerAsync(source,
                VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                    .WithLocation(0).WithArguments("Delay"));
        }
        else
        {
            await VerifyCS.VerifyAnalyzerAsync(source);
        }
    }
}
