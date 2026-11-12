using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleCancellationTokenControlFlowAnalyzerTests
{
    public static IEnumerable<object[]> LoopCases()
    {
        foreach (var token in new[] { "token", "cancellationToken" })
        {
            foreach (var exit in new[] { "break;", "return null!;", "throw new System.Exception();" })
            {
                yield return ["exclusive-" + token + exit, $$"""
                    var original = cancellationToken;
                    var token = cancellationToken;
                    for (var i = 0; i < 2; i++)
                    {
                        if (context is null) {{token}} = CancellationToken.None;
                        else await CALL(1, {{token}});
                        {{token}} = original;
                        {{exit}}
                    }
                    """, false];
                yield return ["opposite-tail-" + token + exit, $$"""
                    var original = cancellationToken;
                    var token = cancellationToken;
                    for (var i = 0; i < 2; i++)
                    {
                        if (context is null) { {{token}} = CancellationToken.None; {{exit}} }
                        else { await CALL(1, {{token}}); {{token}} = original; }
                    }
                    """, false];
            }

            yield return ["real-back-edge-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) {{token}} = CancellationToken.None;
                    else await CALL(1, {{token}});
                }
                """, true];
            yield return ["continue-" + token, $$"""
                var original = cancellationToken;
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) { {{token}} = CancellationToken.None; continue; }
                    else await CALL(1, {{token}});
                    {{token}} = original;
                }
                """, true];
            yield return ["head-reset-" + token, $$"""
                var original = cancellationToken;
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    {{token}} = original;
                    if (context is null) {{token}} = CancellationToken.None;
                    else await CALL(1, {{token}});
                }
                """, false];
            yield return ["tail-reset-" + token, $$"""
                var original = cancellationToken;
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) {{token}} = CancellationToken.None;
                    else await CALL(1, {{token}});
                    {{token}} = original;
                }
                """, false];
            yield return ["self-reset-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    {{token}} = {{token}};
                    if (context is null) {{token}} = CancellationToken.None;
                    else await CALL(1, {{token}});
                }
                """, true];
            yield return ["conditional-exit-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) {{token}} = CancellationToken.None;
                    else await CALL(1, {{token}});
                    if (context is null) break;
                }
                """, true];
            yield return ["nested-break-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) { {{token}} = CancellationToken.None; do { break; } while (true); }
                    else await CALL(1, {{token}});
                }
                """, true];
            yield return ["nested-continue-outer-break-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) { {{token}} = CancellationToken.None; for (var j = 0; j < 2; j++) continue; }
                    else await CALL(1, {{token}});
                    break;
                }
                """, false];
            yield return ["outer-carry-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    for (var j = 0; j < 2; j++)
                    {
                        if (context is null) {{token}} = CancellationToken.None;
                        else await CALL(1, {{token}});
                        break;
                    }
                }
                """, true];
        }

        foreach (var dependency in new[] { "cancellationToken", "input" })
        {
            yield return ["do-condition-" + dependency, $$"""
                var input = cancellationToken;
                var token = cancellationToken;
                do
                {
                    token = {{dependency}};
                    if (context is null) token = CancellationToken.None;
                    else await CALL(1, token);
                } while (({{dependency}} = CancellationToken.None).CanBeCanceled);
                """, true];
            yield return ["for-iterator-" + dependency, $$"""
                var input = cancellationToken;
                var token = cancellationToken;
                for (var i = 0; i < 2; i++, {{dependency}} = token)
                {
                    token = {{dependency}};
                    if (context is null) token = CancellationToken.None;
                    else await CALL(1, token);
                }
                """, true];
            yield return ["for-initializer-" + dependency, $$"""
                var original = cancellationToken;
                var input = cancellationToken;
                var token = cancellationToken;
                for ({{dependency}} = original; context is not null;)
                {
                    token = {{dependency}};
                    if (context is null) token = CancellationToken.None;
                    else await CALL(1, token);
                }
                """, false];
            yield return ["independent-refresh-" + dependency, $$"""
                var original = cancellationToken;
                var input = cancellationToken;
                var token = cancellationToken;
                for (var i = 0; i < 2; i++, {{dependency}} = token)
                {
                    {{dependency}} = original;
                    token = {{dependency}};
                    if (context is null) token = CancellationToken.None;
                    else await CALL(1, token);
                }
                """, false];
        }

        foreach (var expression in new[] { "Pass(cancellationToken)", "cancellationToken.PassToken()", "Pass(Pass(cancellationToken))", "TokenExtensions.PassToken(cancellationToken)", "Pick(default, cancellationToken)", "Pick(token: cancellationToken, ignored: default)", "LoopPass(cancellationToken)" })
        {
            yield return ["helper-result-" + expression, $$"""
                var token = {{expression}};
                for (var i = 0; i < 2; i++) await CALL(1, token);
                """, false];
            yield return ["helper-reset-" + expression, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    cancellationToken = default;
                    token = {{expression}};
                    await CALL(1, token);
                }
                """, true];
        }

        yield return ["helper-selects-flowing-inputs", """
            var token = SelectToken(cancellationToken, cancellationToken, context is null);
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, false];
        yield return ["helper-selects-unrelated-input", """
            var token = SelectToken(cancellationToken, default, context is null);
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, true];
        yield return ["helper-discards-parameter", """
            var token = Drop(cancellationToken);
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, true];
        yield return ["helper-restores-parameter", """
            var token = Restore(cancellationToken);
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, false];
        yield return ["recursive-helper-swaps-inputs", """
            var token = Swap(cancellationToken, default, 1);
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, true];
        yield return ["recursive-helper-discards-input", """
            var token = RecursiveDrop(cancellationToken, 1);
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, true];
        yield return ["helper-discards-local-copy", """
            var token = DropCopy(cancellationToken);
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, true];

        foreach (var call in new[] { "CycleA(cancellationToken, default, 2)", "CycleExpressionA(cancellationToken, 2)" })
        {
            yield return ["helper-cycle-" + call, $$"""
                var token = {{call}};
                for (var i = 0; i < 2; i++) await CALL(1, token);
                """, false];
        }
        foreach (var call in new[] { "CycleSwapA(cancellationToken, default, 2)", "CycleDropA(cancellationToken, 2)" })
        {
            yield return ["helper-cycle-invalid-" + call, $$"""
                var token = {{call}};
                for (var i = 0; i < 2; i++) await CALL(1, token);
                """, true];
        }

        yield return ["forty-conditional-copies", string.Join("\n", Enumerable.Range(0, 40).Select(i =>
            $"var copy{i} = context is null ? {(i == 0 ? "cancellationToken" : $"copy{i - 1}")} : {(i == 0 ? "cancellationToken" : $"copy{i - 1}")};"))
            + "for (var i = 0; i < 2; i++) await CALL(1, copy39);", false];
        yield return ["linked-source-in-loop", """
            for (var i = 0; i < 2; i++)
            {
                using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                var token = linked.Token;
                await CALL(1, token);
            }
            """, false];
        yield return ["conditional-snapshot-safe", """
            var snapshot = context is null ? cancellationToken : cancellationToken;
            var token = cancellationToken;
            for (var i = 0; i < 2; i++)
            {
                token = snapshot;
                if (context is null) token = CancellationToken.None;
                else await CALL(1, token);
            }
            """, false];
        yield return ["conditional-snapshot-unsafe", """
            var snapshot = context is null ? cancellationToken : CancellationToken.None;
            for (var i = 0; i < 2; i++) await CALL(1, snapshot);
            """, true];
    }

    [TestMethod]
    [DynamicData(nameof(LoopCases))]
    public async Task Tracks_Loop_Reaching_Values(string name, string body, bool warning)
    {
        _ = name;
        await Verify(body, warning);
    }

    public static IEnumerable<object[]> ExceptionCases()
    {
        yield return ["filter-before-finalizer-discards", """
            var token = cancellationToken;
            try { try { throw new System.Exception(); } finally { token = default; } }
            catch (System.Exception) when ((token = cancellationToken).CanBeCanceled) { }
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, true];
        yield return ["filter-before-finalizer-restores", """
            var token = default(CancellationToken);
            try { try { throw new System.Exception(); } finally { token = cancellationToken; } }
            catch (System.Exception) when ((token = default).CanBeCanceled) { }
            catch { }
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, false];
        yield return ["rejected-filter-before-finalizer", """
            var token = cancellationToken;
            try { try { throw new System.Exception(); } finally { token = default; } }
            catch (System.Exception) when ((token = cancellationToken) is var ignored && false) { return null!; }
            catch { }
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, true];
        yield return ["throwing-filter-before-finalizer", """
            var token = cancellationToken;
            try { try { throw new System.Exception(); } finally { token = default; } }
            catch (System.Exception) when ((token = cancellationToken) is var ignored && ThrowFilter()) { return null!; }
            catch { }
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, true];
        yield return ["nested-finalizers-after-filter", """
            var token = cancellationToken;
            try
            {
                try { try { throw new System.Exception(); } finally { token = cancellationToken; } }
                finally { token = default; }
            }
            catch (System.Exception) when ((token = cancellationToken).CanBeCanceled) { }
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, true];
        yield return ["outer-filter-after-rejected-inner-filter-before-cleanup", """
            var token = cancellationToken;
            try
            {
                try
                {
                    try { throw new System.Exception(); } finally { token = default; }
                }
                catch (System.Exception) when ((token = cancellationToken) is var ignored && false) { return null!; }
            }
            catch (System.Exception) when (token.CanBeCanceled) { }
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, true];
        yield return ["throwing-finalizer-replaces-selected-handler", """
            var token = cancellationToken;
            try
            {
                try { throw new System.Exception(); }
                finally { token = default; throw new System.InvalidOperationException(); }
            }
            catch (System.InvalidOperationException) { }
            catch (System.Exception) when ((token = cancellationToken).CanBeCanceled) { return null!; }
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, true];
        yield return ["rethrow-filter-before-finalizer", """
            var token = cancellationToken;
            try
            {
                try { try { throw new System.Exception(); } catch { throw; } }
                finally { token = default; }
            }
            catch (System.Exception) when ((token = cancellationToken).CanBeCanceled) { }
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, true];
        yield return ["ref-getter-before-rhs", "var token = default(CancellationToken); try { Slot = (token = cancellationToken); } catch { } await CALL(1, token);", true];
        yield return ["ref-getter-preserves-input", "var token = cancellationToken; try { Slot = default; } catch { } await CALL(1, token);", false];
        yield return ["ref-method-before-rhs", "var token = default(CancellationToken); try { ThrowSlot() = (token = cancellationToken).GetHashCode(); } catch { } await CALL(1, token);", true];
        yield return ["ref-method-before-tuple-rhs", "var token = default(CancellationToken); try { (ThrowSlot(), _) = ((token = cancellationToken).GetHashCode(), 0); } catch { } await CALL(1, token);", true];
        foreach (var expression in new[] { "target.Run()", "target.Value", "target[0]", "(int)target", "target + 1", "-target" })
        {
            yield return ["dynamic-throw-" + expression, $$"""
                var token = cancellationToken;
                dynamic target = context;
                try { var result = {{expression}}; }
                catch { token = default; }
                await CALL(1, token);
                """, true];
        }

        foreach (var modifier in new[] { "ref", "out" })
        {
            yield return ["dynamic-write-" + modifier, $$"""
                var token = cancellationToken;
                dynamic target = context;
                try { target.Run({{modifier}} token); }
                catch { }
                await CALL(1, token);
                """, true];
        }

        yield return ["unboxing-throw", """
            var token = cancellationToken;
            object boxed = context;
            try { var number = (int)boxed; }
            catch { token = default; }
            await CALL(1, token);
            """, true];
        foreach (var filter in new[] { "(token = default) is var ignored && context is null", "(token = default).CanBeCanceled", "(token = default) is var ignored && context.ToString() is null" })
        {
            yield return ["false-filter-writes-" + filter, $$"""
                var token = cancellationToken;
                try { throw new System.Exception(); }
                catch (System.Exception) when ({{filter}}) { return null!; }
                catch { }
                await CALL(1, token);
                """, true];
        }

        yield return ["filter-writes-reach-outer-handler", """
            var token = cancellationToken;
            try
            {
                try { throw new System.Exception(); }
                catch (System.Exception) when ((token = default) is var ignored && context is null) { return null!; }
            }
            catch { }
            await CALL(1, token);
            """, true];
        yield return ["true-filter-does-not-reach-sibling", """
            var token = cancellationToken;
            try { throw new System.Exception(); }
            catch (System.Exception) when (true) { token = cancellationToken; }
            catch { token = default; }
            await CALL(1, token);
            """, false];

        yield return ["typed-true-filter-can-be-skipped", """
            var token = cancellationToken;
            try { throw new System.Exception(); }
            catch (System.InvalidOperationException) when (true) { return null!; }
            catch { token = default; }
            await CALL(1, token);
            """, true];
        yield return ["discard-does-not-throw", """
            var token = cancellationToken;
            try { _ = context; }
            catch { token = default; }
            await CALL(1, token);
            """, false];
        yield return ["throwing-filter-retains-writes", """
            var token = cancellationToken;
            try { throw new System.Exception(); }
            catch (System.Exception) when ((token = default) is var ignored && ThrowFilter()) { return null!; }
            catch { }
            await CALL(1, token);
            """, true];
        yield return ["false-filter-restores", """
            var token = default(CancellationToken);
            try { throw new System.Exception(); }
            catch (System.Exception) when ((token = cancellationToken) is var ignored && false) { return null!; }
            catch { }
            await CALL(1, token);
            """, false];
        yield return ["recursive-loop-helper-is-bounded", """
            var token = RecursivePass(cancellationToken, 2);
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, false];

        yield return ["return-does-not-throw", """
            var token = cancellationToken;
            try { if (context is null) { token = default; return null!; } }
            catch { }
            await CALL(1, token);
            """, false];
        yield return ["throwing-path", """
            var token = cancellationToken;
            try { token = default; throw new System.Exception(); }
            catch { }
            await CALL(1, token);
            """, true];
        yield return ["catch-restores", """
            var token = cancellationToken;
            try { token = default; throw new System.Exception(); }
            catch { token = cancellationToken; }
            await CALL(1, token);
            """, false];
        yield return ["sibling-rethrow", """
            var token = cancellationToken;
            try { if (context is null) { token = default; throw new System.InvalidOperationException(); } }
            catch (System.InvalidOperationException) { throw; }
            catch { }
            await CALL(1, token);
            """, true];
        yield return ["finally-restores", """
            var token = cancellationToken;
            try { token = default; }
            finally { token = cancellationToken; }
            await CALL(1, token);
            """, false];
        yield return ["finally-throws-to-catch", """
            var token = cancellationToken;
            try { try { token = default; return null!; } finally { throw new System.Exception(); } }
            catch { }
            await CALL(1, token);
            """, true];
        yield return ["sibling-catch-rethrow-does-not-continue", """
            var token = cancellationToken;
            try { _ = context.ToString(); }
            catch (System.InvalidOperationException) { token = default; throw; }
            catch { }
            await CALL(1, token);
            """, false];
        yield return ["nested-catch-restores-before-outer", """
            var token = cancellationToken;
            try
            {
                try { token = default; throw new System.Exception(); }
                catch { token = cancellationToken; }
            }
            catch { }
            await CALL(1, token);
            """, false];
        yield return ["nested-finally-restores-before-catch", """
            var token = cancellationToken;
            try
            {
                try { token = default; throw new System.Exception(); }
                finally { token = cancellationToken; }
            }
            catch { }
            await CALL(1, token);
            """, false];
        foreach (var cleanup in new[] { "using (var resource = new Resource())", "await using (var resource = new Resource())",
            "foreach (var item in new Sequence())", "await foreach (var item in new Sequence())" })
        {
            foreach (var handler in new[] { "", "return null!;", "token = cancellationToken;" })
            {
                yield return [cleanup + handler, $$"""
                    var token = cancellationToken;
                    try { {{cleanup}} { token = default; return null!; } }
                    catch { {{handler}} }
                    await CALL(1, token);
                    """, handler.Length == 0];
            }
        }

        foreach (var collection in new[] { "new[] { 1 }", "\"text\"" })
        {
            yield return ["no-dispose-" + collection, $$"""
                var token = cancellationToken;
                try { foreach (var item in {{collection}}) { token = default; return null!; } }
                catch { }
                await CALL(1, token);
                """, false];
        }
    }

    public static IEnumerable<object[]> HelperCases()
    {
        yield return ["loop-reset", "for (var i = 0; i < 2; i++) { } token = default; return token;", true];
        yield return ["straight-line-reset", "token = default; return token;", true];
        yield return ["conditional-reset", "if (token.CanBeCanceled) token = default; return token;", true];
        yield return ["loop-carried-reset", "for (var i = 0; i < 2; i++) { if (i == 1) token = default; } return token;", true];
        yield return ["self-assignment", "for (var i = 0; i < 2; i++) token = token; return token;", false];
        yield return ["restored-copy", "var original = token; token = default; token = original; return token;", false];
        yield return ["saved-copy", "var original = token; token = default; return original;", false];
        yield return ["replaced-local", "var copy = token; for (var i = 0; i < 2; i++) copy = default; return copy;", true];
        yield return ["catch-reset", "try { throw new System.Exception(); } catch { token = default; } return token;", true];
        yield return ["finally-reset", "try { } finally { token = default; } return token;", true];
        yield return ["finally-restores", "var original = token; try { token = default; } finally { token = original; } return token;", false];
        yield return ["nested-helper", "return Pass(token);", false];
        yield return ["ref-write", "void Reset(ref CancellationToken value) { value = default; } Reset(ref token); return token;", true];
        yield return ["out-write", "void Reset(out CancellationToken value) { value = default; } Reset(out token); return token;", true];
        yield return ["nested-reset", "return Drop(token);", true];
        yield return ["extension-reset", "return token.DropToken();", true];
        yield return ["return-snapshot-before-finally", "try { return token; } finally { token = default; }", false];
        yield return ["heap-ref-write", "var items = new CancellationToken[1]; ref var item = ref items[0]; item = default; return token;", false];
        yield return ["unrelated-int-array-ref", "var numbers = new int[1]; ref var item = ref numbers[0]; item = 1; return token;", false];
        yield return ["heap-ref-call", "void Reset(ref CancellationToken value) { value = default; } var items = new CancellationToken[1]; ref var item = ref items[0]; Reset(ref item); return token;", false];
        yield return ["field-ref-write", "var box = new TokenBox(); ref var item = ref box.Value; item = default; return token;", false];
        yield return ["unrelated-int-unknown-ref", "static ref int Identity(ref int value) => ref value; int number = 0; ref var item = ref Identity(ref number); item = 1; return token;", false];
        yield return ["unknown-ref-write", "static ref CancellationToken Identity(ref CancellationToken value) => ref value; ref var item = ref Identity(ref token); item = default; return token;", true];
        yield return ["heap-ref-read", "var items = new CancellationToken[1]; ref var item = ref items[0]; item = token; return item;", true];
        yield return ["mixed-heap-local-ref-write", "var items = new CancellationToken[1]; ref var item = ref (token.CanBeCanceled ? ref token : ref items[0]); item = default; return token;", true];
        yield return ["mixed-heap-local-ref-restore", "var original = token; token = default; var items = new CancellationToken[1]; ref var item = ref (original.CanBeCanceled ? ref token : ref items[0]); item = original; return token;", true];
        yield return ["recursive-result-not-used", "_ = RecursivePass(token, 2); return token;", false];
    }

    [TestMethod]
    [DynamicData(nameof(HelperCases))]
    public async Task Tracks_Helper_Parameter_Writes(string name, string body, bool warning)
    {
        _ = name;
        await Verify("""
            var token = WithWrites(cancellationToken);
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, warning, $$"""
            private static CancellationToken WithWrites(CancellationToken token) { {{body}} }
            """);
    }

    [TestMethod]
    [DataRow("DateTime.UtcNow.Ticks > 0 ? first : third", 0, 2, false)]
    [DataRow("DateTime.UtcNow.Ticks switch { > 0 => first, _ => third }", 0, 2, false)]
    [DataRow("DateTime.UtcNow.Ticks > 0 ? first : third", 2, 2, true)]
    [DataRow("DateTime.UtcNow.Ticks switch { > 0 => first, _ => third }", 2, 2, true)]
    [DataRow("DateTime.UtcNow.Ticks > 0 ? first : default", 0, 2, true)]
    [DataRow("DateTime.UtcNow.Ticks switch { > 0 => first, _ => default(CancellationToken) }", 0, 2, true)]
    public async Task Proves_Scalar_Helper_First_Loop_Entry(string expression, int initial, int limit, bool warning)
    {
        await Verify("""
            var first = cancellationToken;
            var second = default(CancellationToken);
            var third = cancellationToken;
            var token = Forward(ref first, ref second, ref third);
            await CALL(1, token);
            """, warning, $$"""
            private static CancellationToken Forward(scoped ref CancellationToken first,
                scoped ref CancellationToken second, scoped ref CancellationToken third)
            {
                for (var i = {{initial}}; i < {{limit}}; i++)
                {
                    first = third;
                    second = {{expression}};
                }
                return second;
            }
            """);
    }

    public static IEnumerable<object[]> RefParameterCases()
    {
        yield return ["conditional-rebind-to-default", "if (first.CanBeCanceled) first = ref second; return first;", true];
        yield return ["conditional-rebind-preserves-source", "ref var saved = ref first; if (first.CanBeCanceled) first = ref saved; return first;", false];
        yield return ["rebind-to-default", "first = ref second; return first;", true];
        yield return ["rebind-to-source", "second = ref first; return second;", false];
        yield return ["rebind-retains-snapshot", "var saved = first; first = ref second; return saved;", false];
        yield return ["rebound-source-reset", "second = ref first; first = default; return second;", true];
        yield return ["rebound-default-restored", "var saved = first; first = ref second; second = saved; return first;", false];
        yield return ["old-alias-keeps-original-storage", "ref var saved = ref first; first = ref second; return saved;", false];
    }

    [TestMethod]
    [DynamicData(nameof(RefParameterCases))]
    public async Task Tracks_Ref_Parameter_Rebindings(string name, string body, bool warning)
    {
        _ = name;
        await Verify("""
            var source = cancellationToken;
            var other = default(CancellationToken);
            var token = Rebind(ref source, ref other);
            for (var i = 0; i < 2; i++) await CALL(1, token);
            """, warning, $$"""
            private static CancellationToken Rebind(ref CancellationToken first, ref CancellationToken second) { {{body}} }
            """);
    }

    public static IEnumerable<object[]> StoreCases()
    {
        yield return ["deconstruct-local", "var token = cancellationToken; (token, _) = (CancellationToken.None, 0);", true];
        yield return ["deconstruct-parameter", "(cancellationToken, _) = (CancellationToken.None, 0); var token = cancellationToken;", true];
        yield return ["deconstruct-preserves", "var token = default(CancellationToken); (token, _) = (cancellationToken, 0);", false];
        yield return ["deconstruct-snapshot", "var token = default(CancellationToken); var saved = cancellationToken; (saved, token) = (default, saved);", false];
        yield return ["deconstruct-swap", "var token = cancellationToken; var other = default(CancellationToken); (token, other) = (other, token);", true];
        yield return ["deconstruct-nested", "var token = cancellationToken; ((token, _), _) = ((default(CancellationToken), 0), 0);", true];
        yield return ["deconstruct-opaque", "(CancellationToken, int) Pair() => (default, 0); var token = cancellationToken; (token, _) = Pair();", true];
        yield return ["ref-binding-without-write", "var token = cancellationToken; var saved = cancellationToken; { ref CancellationToken alias = ref token; saved = alias; } token = saved;", false];
        yield return ["ref-local-write", "var token = cancellationToken; { ref CancellationToken alias = ref token; alias = default; }", true];
        yield return ["ref-parameter-write", "{ ref CancellationToken alias = ref cancellationToken; alias = default; } var token = cancellationToken;", true];
        yield return ["ref-chain-write", "var token = cancellationToken; { ref CancellationToken alias = ref token; ref CancellationToken second = ref alias; second = default; }", true];
        yield return ["ref-rebind", "var token = cancellationToken; var other = cancellationToken; { ref CancellationToken alias = ref token; alias = ref other; alias = default; }", false];
        yield return ["ref-rebind-retains-old-alias", "var token = cancellationToken; var other = cancellationToken; { ref CancellationToken alias = ref token; ref CancellationToken second = ref alias; alias = ref other; second = default; }", true];
        yield return ["ref-restore", "var token = default(CancellationToken); { ref CancellationToken alias = ref token; alias = cancellationToken; }", false];
        yield return ["ref-read-after-backing-write", "var token = cancellationToken; var saved = cancellationToken; { ref CancellationToken alias = ref token; token = default; saved = alias; } token = saved;", true];
        yield return ["ref-argument", "void Reset(ref CancellationToken value) { value = default; } var token = cancellationToken; { ref CancellationToken alias = ref token; Reset(ref alias); }", true];
        yield return ["out-argument", "void Reset(out CancellationToken value) { value = default; } var token = cancellationToken; { ref CancellationToken alias = ref token; Reset(out alias); }", true];
        yield return ["deconstruct-ref-target", "var token = cancellationToken; { ref CancellationToken alias = ref token; (alias, _) = (default(CancellationToken), 0); }", true];
        yield return ["ref-conditional-write", "var token = cancellationToken; var other = cancellationToken; { ref CancellationToken alias = ref (context is null ? ref token : ref other); alias = default; }", true];
        yield return ["constructor-ref-write", "var token = cancellationToken; _ = new TokenResetter(ref token);", true];
        yield return ["constructor-out-write", "var token = cancellationToken; _ = new TokenResetter(out token, false);", true];
        yield return ["constructor-throw-retains-write", "var token = cancellationToken; try { _ = new TokenResetter(out token, true); } catch { }", true];
        yield return ["constructor-initializer-restores", "var token = cancellationToken; _ = new TokenResetter(ref token) { Marker = (token = cancellationToken).CanBeCanceled };", false];
        yield return ["ref-field-rebinding-preserves-local", "var token = cancellationToken; { scoped TokenRefHolder holder = default; holder.Token = ref token; }", false];
        yield return ["ref-field-rebinding-preserves-parameter", "{ scoped TokenRefHolder holder = default; holder.Token = ref cancellationToken; } var token = cancellationToken;", false];
        yield return ["ref-field-rebound-write-invalidates-local", "var token = cancellationToken; { scoped TokenRefHolder holder = default; holder.Token = ref token; holder.Token = default; }", true];
        yield return ["direct-ref-field-invalidates-local", "var token = cancellationToken; { var holder = new TokenRefHolder(ref token); holder.Token = default; }", true];
        yield return ["deconstruct-unboxing-throw", "var token = cancellationToken; object boxed = context; try { CancellationToken ignored; int number; (ignored, number) = ((CancellationToken, int))boxed; } catch { token = default; }", true];
        yield return ["deconstruct-tuple-conversion-throw", "var token = cancellationToken; try { CancellationToken ignored; int number; decimal amount = decimal.MaxValue; (ignored, number) = ((CancellationToken, int))(cancellationToken, amount); } catch { token = default; }", true];
        yield return ["deconstruct-rhs-before-null-store", "var token = default(CancellationToken); CancellationToken[] items = null!; try { (items[0], _) = ((token = cancellationToken), 0); } catch { }", false];
        yield return ["deconstruct-dynamic-target-effects", "var token = cancellationToken; dynamic target = context; try { (target[(token = default)], _) = (1, 0); } catch { }", true];
    }

    [TestMethod]
    [DynamicData(nameof(StoreCases))]
    public async Task Tracks_Stores_And_Ref_Aliases(string name, string body, bool warning)
    {
        _ = name;
        await Verify(body + "\nfor (var i = 0; i < 2; i++) await CALL(1, token);", warning);
    }

    public static IEnumerable<object[]> CollectionCases()
    {
        yield return ["atomic-element-restores-source", "var token = cancellationToken; try { System.Collections.Generic.List<bool> items = [(token = default) == (token = cancellationToken)]; } catch { }", false];
        yield return ["branching-atomic-element-restores-source", "var token = cancellationToken; try { System.Collections.Generic.List<bool> items = [context is null ? (token = default) == (token = cancellationToken) : (token = default) == (token = cancellationToken)]; } catch { }", false];
        yield return ["later-atomic-element-restores-source", "var token = cancellationToken; try { System.Collections.Generic.List<bool> items = [true, (token = default) == (token = cancellationToken)]; } catch { }", false];
        yield return ["add-before-next-element", "var token = cancellationToken; try { System.Collections.Generic.List<bool> items = [(token = default) == default, (token = cancellationToken) == default]; return null!; } catch { }", true];
        yield return ["repeated-constructor-boundary", "var token = cancellationToken; for (var j = 0; j < 2; j++) { try { System.Collections.Generic.List<bool> items = [(token = cancellationToken) == default]; } catch { break; } token = default; }", true];
        yield return ["constructor-before-element", "var token = default(CancellationToken); try { ThrowingCollection items = [(token = cancellationToken).CanBeCanceled]; return null!; } catch { }", true];
        yield return ["constructor-before-branching-element", "var token = default(CancellationToken); try { ThrowingCollection items = [context is null ? (token = cancellationToken).CanBeCanceled : (token = cancellationToken).CanBeCanceled]; return null!; } catch { }", true];
        yield return ["constructor-preserves-source", "var token = cancellationToken; try { ThrowingCollection items = [token.CanBeCanceled]; return null!; } catch { }", false];
        yield return ["spread-before-later-element", "var sequence = new BoolSequence(); var token = default(CancellationToken); try { bool[] items = [..sequence, (token = cancellationToken).CanBeCanceled]; return null!; } catch { }", true];
        yield return ["array-element-before-completion", "var token = default(CancellationToken); try { bool[] items = [(token = cancellationToken).CanBeCanceled]; return null!; } catch { }", false];
    }

    [TestMethod]
    [DynamicData(nameof(CollectionCases))]
    public async Task Tracks_Collection_Call_Order(string name, string body, bool warning)
    {
        _ = name;
        await Verify(body + "\nfor (var i = 0; i < 2; i++) await CALL(1, token);", warning, """
            public class ThrowingCollection : System.Collections.Generic.List<bool>
            {
                public ThrowingCollection() => throw new System.Exception();
            }
            public class BoolSequence : System.Collections.Generic.IEnumerable<bool>
            {
                public System.Collections.Generic.IEnumerator<bool> GetEnumerator() => throw new System.Exception();
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
            }
            """);
    }

    public static IEnumerable<object[]> TypedExceptionCases()
    {
        yield return ["incompatible-catch", "try { _ = 1 / divisor; } catch (System.FormatException) { token = default; }", false];
        yield return ["matching-catch", "try { _ = 1 / divisor; } catch (System.DivideByZeroException) { token = default; }", true];
        yield return ["base-catch", "try { _ = 1 / divisor; } catch (System.ArithmeticException) { token = default; }", true];
        yield return ["overflow-catch", "try { _ = 1 / divisor; } catch (System.OverflowException) { token = default; }", true];
        yield return ["first-matching-catch", "try { _ = 1 / divisor; } catch (System.ArithmeticException) { } catch (System.Exception) { token = default; }", false];
        yield return ["incompatible-filter", "try { _ = 1 / divisor; } catch (System.FormatException) when ((token = default) == default) { }", false];
        yield return ["matching-filter", "try { _ = 1 / divisor; } catch (System.ArithmeticException) when ((token = default) == default) { }", true];
        yield return ["rejected-filter", "try { _ = 1 / divisor; } catch (System.ArithmeticException) when (false) { token = default; }", false];
        yield return ["outer-incompatible-catch", "try { try { _ = 1 / divisor; } catch (System.FormatException) { token = default; } } catch (System.ArithmeticException) { }", false];
        yield return ["unknown-call", "try { ThrowFilter(); } catch (System.FormatException) { token = default; }", true];
    }

    [TestMethod]
    [DynamicData(nameof(TypedExceptionCases))]
    public async Task Selects_Compatible_Exception_Handlers(string name, string body, bool warning)
    {
        _ = name;
        await Verify("var token = cancellationToken; int divisor = context is null ? 0 : 1; " + body
            + "\nfor (var i = 0; i < 2; i++) await CALL(1, token);", warning);
    }

    public static IEnumerable<object[]> StoreExceptionCases()
    {
        yield return ["struct-field-deconstruction", "var box = default(StoreBox); try { (box.Value, _) = (1, 2); } catch { token = default; }", false];
        yield return ["struct-field", "var box = default(StoreBox); try { box.Value = 1; } catch { token = default; }", false];
        yield return ["field-incompatible-catch", "TokenBox box = null!; try { box.Value = default; } catch (System.FormatException) { token = default; }", false];
        yield return ["field-matching-catch", "TokenBox box = null!; try { box.Value = default; } catch (System.NullReferenceException) { token = default; }", true];
        yield return ["array-incompatible-catch", "int[] items = null!; try { items[0] = 1; } catch (System.FormatException) { token = default; }", false];
        yield return ["array-matching-catch", "int[] items = null!; try { items[0] = 1; } catch (System.IndexOutOfRangeException) { token = default; }", true];
        yield return ["setter-unknown-exception", "StoreBox box = default; try { box.Property = 1; } catch (System.FormatException) { token = default; }", true];
        yield return ["rhs-before-field-store", "TokenBox box = null!; var token = default(CancellationToken); try { box.Value = token = cancellationToken; } catch { }", false];
    }

    public static IEnumerable<object[]> RefFieldExceptionCases()
    {
        foreach (var modifier in new[] { "ref", "out", "in" })
        {
            yield return [modifier + "-argument-address", "token = default; try { Consume_" + modifier + "(" + modifier + " box.Value, (token = cancellationToken).CanBeCanceled); } catch { }", false];
        }

        yield return ["null-write", "try { box.Value = 1; } catch { token = default; }", true];
        yield return ["null-write-matching-catch", "try { box.Value = 1; } catch (System.NullReferenceException) { token = default; }", true];
        yield return ["null-write-incompatible-catch", "try { box.Value = 1; } catch (System.FormatException) { token = default; }", false];
        yield return ["null-deconstruction", "try { (box.Value, _) = (1, 2); } catch { token = default; }", true];
        yield return ["null-compound-write", "try { box.Value += 1; } catch { token = default; }", true];
        yield return ["null-read", "try { _ = box.Value; } catch { token = default; }", true];
        yield return ["copy-null-reference", "try { ref int alias = ref box.Value; } catch { token = default; }", false];
        yield return ["rebind", "int value = 0; try { box.Value = ref value; } catch { token = default; }", false];
        yield return ["rhs-before-deconstruction", "token = default; try { (box.Value, _) = ((token = cancellationToken).CanBeCanceled ? 1 : 0, 0); } catch { }", false];
        yield return ["rhs-before-write", "token = default; try { box.Value = (token = cancellationToken).CanBeCanceled ? 1 : 0; } catch { }", false];
    }

    [TestMethod]
    [DynamicData(nameof(RefFieldExceptionCases))]
    public async Task Classifies_Ref_Field_Exceptions(string name, string body, bool warning)
    {
        _ = name;
        await Verify("var token = cancellationToken; scoped RefStoreBox box = default; " + body
            + "\nfor (var i = 0; i < 2; i++) await CALL(1, token);", warning,
            "private ref struct RefStoreBox { public ref int Value; } private static void Consume_ref(ref int value, bool restored) { } private static void Consume_out(out int value, bool restored) { value = 0; } private static void Consume_in(in int value, bool restored) { }");
    }

    [TestMethod]
    [DynamicData(nameof(StoreExceptionCases))]
    public async Task Classifies_Store_Exceptions(string name, string body, bool warning)
    {
        _ = name;
        var prefix = name == "rhs-before-field-store" ? "" : "var token = cancellationToken; ";
        await Verify(prefix + body + "\nfor (var i = 0; i < 2; i++) await CALL(1, token);", warning,
            "private struct StoreBox { public int Value; public int Property { set { throw new System.FormatException(); } } }");
    }

    [TestMethod]
    [DynamicData(nameof(ExceptionCases))]
    public async Task Tracks_Exception_And_Cleanup_Values(string name, string body, bool warning)
    {
        _ = name;
        await Verify(body, warning);
    }

    [TestMethod]
    [DataRow("cancellationToken", false)]
    [DataRow("default(CancellationToken)", true)]
    public async Task Tracks_Nullable_Coalescing_Values(string initial, bool warning)
    {
        await Verify("CancellationToken? optional = " + initial
            + "; var token = optional ?? cancellationToken; for (var i = 0; i < 2; i++) await CALL(1, token);", warning);
    }

    [TestMethod]
    [DataRow("(CancellationToken?)cancellationToken", false)]
    [DataRow("(CancellationToken?)default(CancellationToken)", true)]
    [DataRow("(CancellationToken?)null", true)]
    public async Task Tracks_Statically_NonNull_Nullable_Coalescing(string expression, bool warning)
    {
        await Verify("var token = " + expression + " ?? CancellationToken.None; for (var i = 0; i < 2; i++) await CALL(1, token);", warning);
    }

    private static async Task Verify(string body, bool warning, string helpers = "")
    {
        body = body.Replace("CALL", warning ? "{|#0:Task.Delay|}" : "Task.Delay");
        // Mark the invocation, rather than only its method name.
        body = body.Replace("{|#0:Task.Delay|}(1, token)", "{|#0:Task.Delay(1, token)|}")
            .Replace("{|#0:Task.Delay|}(1, cancellationToken)", "{|#0:Task.Delay(1, cancellationToken)|}")
            .Replace("{|#0:Task.Delay|}(1, snapshot)", "{|#0:Task.Delay(1, snapshot)|}")
            .Replace("{|#0:Task.Delay|}(1, copy39)", "{|#0:Task.Delay(1, copy39)|}");
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeaderWithExtensions}}
            public class BuildModule : Module<List<string>>
            {
                {{helpers}}
                private static ref int ThrowSlot() => throw new System.Exception();
                private static CancellationToken SelectToken(CancellationToken first, CancellationToken second, bool chooseFirst) => chooseFirst ? first : second;
                private static CancellationToken Pass(CancellationToken token) => token;
                private static CancellationToken Pick(CancellationToken ignored, CancellationToken token) => token;
                private static bool ThrowFilter() => throw new System.Exception();
                private static CancellationToken LoopPass(CancellationToken token)
                {
                    for (var i = 0; i < 2; i++) { }
                    return token;
                }
                private static CancellationToken Drop(CancellationToken token)
                {
                    for (var i = 0; i < 2; i++) { }
                    token = default;
                    return token;
                }
                private static CancellationToken Restore(CancellationToken token)
                {
                    var original = token;
                    for (var i = 0; i < 2; i++) { }
                    token = default;
                    token = original;
                    return token;
                }
                private static CancellationToken DropCopy(CancellationToken token)
                {
                    var copy = token;
                    for (var i = 0; i < 2; i++) { }
                    copy = default;
                    return copy;
                }
                private static CancellationToken Swap(CancellationToken token, CancellationToken other, int remaining)
                {
                    for (var i = 0; i < remaining; i++) token = Swap(other, token, remaining - 1);
                    return token;
                }
                private static CancellationToken RecursiveDrop(CancellationToken token, int remaining)
                {
                    for (var i = 0; i < remaining; i++) token = RecursiveDrop(token, remaining - 1);
                    token = default;
                    return token;
                }
                private static CancellationToken RecursivePass(CancellationToken token, int remaining)
                {
                    for (var i = 0; i < remaining; i++) token = RecursivePass(token, remaining - 1);
                    return token;
                }
                private static CancellationToken CycleA(CancellationToken token, CancellationToken other, int remaining)
                {
                    for (var i = 0; i < remaining; i++) token = CycleB(remaining - 1, other, token);
                    return token;
                }
                private static CancellationToken CycleB(int remaining, CancellationToken other, CancellationToken token)
                {
                    for (var i = 0; i < remaining; i++) token = CycleA(token, other, remaining - 1);
                    return token;
                }
                private static CancellationToken CycleExpressionA(CancellationToken token, int remaining)
                    => remaining > 0 ? CycleExpressionB(token, remaining - 1) : token;
                private static CancellationToken CycleExpressionB(CancellationToken token, int remaining)
                    => remaining > 0 ? CycleExpressionA(token, remaining - 1) : token;
                private static CancellationToken CycleSwapA(CancellationToken token, CancellationToken other, int remaining)
                {
                    for (var i = 0; i < remaining; i++) token = CycleSwapB(other, token, remaining - 1);
                    return token;
                }
                private static CancellationToken CycleSwapB(CancellationToken token, CancellationToken other, int remaining)
                {
                    for (var i = 0; i < remaining; i++) token = CycleSwapA(token, other, remaining - 1);
                    return token;
                }
                private static CancellationToken CycleDropA(CancellationToken token, int remaining)
                {
                    for (var i = 0; i < remaining; i++) token = CycleDropB(token, remaining - 1);
                    return token;
                }
                private static CancellationToken CycleDropB(CancellationToken token, int remaining)
                {
                    for (var i = 0; i < remaining; i++) token = CycleDropA(token, remaining - 1);
                    token = default;
                    return token;
                }

                private static ref CancellationToken Slot => throw new System.Exception();

                protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                {
                    {{body}}
                    return null!;
                }
            }
            public static class TokenExtensions
            {
                public static CancellationToken PassToken(this CancellationToken token) => token;
                public static CancellationToken DropToken(this CancellationToken token)
                {
                    token = default;
                    return token;
                }
            }
            public class TokenBox
            {
                public CancellationToken Value;
            }
            public class Resource : System.IDisposable, System.IAsyncDisposable
            {
                public void Dispose() => throw new System.Exception();
                public System.Threading.Tasks.ValueTask DisposeAsync() => throw new System.Exception();
            }
            public ref struct TokenRefHolder
            {
                public ref CancellationToken Token;
                public TokenRefHolder(ref CancellationToken token) { Token = ref token; }
            }
            public class TokenResetter
            {
                public bool Marker { get; set; }
                public TokenResetter(ref CancellationToken token) { token = default; }
                public TokenResetter(out CancellationToken token, bool fail)
                {
                    token = default;
                    if (fail) throw new System.Exception();
                }
            }
            public class Sequence : System.Collections.Generic.IEnumerable<int>, System.Collections.Generic.IAsyncEnumerable<int>
            {
                public System.Collections.Generic.IEnumerator<int> GetEnumerator() => throw new System.Exception();
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
                public System.Collections.Generic.IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default) => throw new System.Exception();
            }
            """;
        if (warning)
        {
            await VerifyCS.VerifyAnalyzerAsync(source, VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
        }
        else
        {
            await VerifyCS.VerifyAnalyzerAsync(source);
        }
    }
}
