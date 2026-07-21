function ConvertTo-UtcDateTimeOffset {
    [CmdletBinding()]
    param(
        [AllowNull()]$Value
    )

    if ($null -eq $Value) {
        return $null
    }

    if ($Value -is [DateTimeOffset]) {
        return ([DateTimeOffset]$Value).ToUniversalTime()
    }

    if ($Value -is [DateTime]) {
        return [DateTimeOffset]::new(([DateTime]$Value).ToUniversalTime())
    }

    $text = [string]$Value
    if ([string]::IsNullOrWhiteSpace($text)) {
        return $null
    }

    try {
        return [DateTimeOffset]::Parse($text).ToUniversalTime()
    }
    catch {
        return $null
    }
}

function Test-ReviewSubmittedBeforeInstant {
    [CmdletBinding()]
    param(
        [AllowNull()]$Review,
        [AllowNull()]$InstantUtc
    )

    $instant = ConvertTo-UtcDateTimeOffset $InstantUtc
    if ($null -eq $instant) {
        return $false
    }

    $submittedAt = ConvertTo-UtcDateTimeOffset $Review.submittedAt
    if ($null -eq $submittedAt) {
        return $false
    }

    return $submittedAt -lt $instant
}

function Test-StatusCheckCompletedAfterInstant {
    [CmdletBinding()]
    param(
        [AllowNull()]$Checks,
        [Parameter(Mandatory)][string]$Name,
        [AllowNull()]$InstantUtc
    )

    $instant = ConvertTo-UtcDateTimeOffset $InstantUtc
    if ($null -eq $instant) {
        return $false
    }

    $allowedConclusions = @('SUCCESS', 'SKIPPED', 'NEUTRAL')
    foreach ($check in @($Checks | Where-Object { $null -ne $_ })) {
        if ([string]$check.name -ne $Name) {
            continue
        }

        if (($null -ne $check.status) -and ([string]$check.status -ne 'COMPLETED')) {
            continue
        }

        if (($null -ne $check.conclusion) -and ($allowedConclusions -notcontains [string]$check.conclusion)) {
            continue
        }

        $completedAt = ConvertTo-UtcDateTimeOffset $check.completedAt
        if ($null -ne $completedAt -and $completedAt -gt $instant) {
            return $true
        }
    }

    return $false
}

function Test-StaleBotReviewCanBeIgnored {
    [CmdletBinding()]
    param(
        [AllowNull()]$Review,
        [AllowNull()]$HeadCommitCommittedAt,
        [AllowNull()]$Checks
    )

    $author = [string]$Review.author.login
    if ($author -notmatch '^claude(?:\[bot\])?$') {
        return $false
    }

    if (-not (Test-ReviewSubmittedBeforeInstant -Review $Review -InstantUtc $HeadCommitCommittedAt)) {
        return $false
    }

    return Test-StatusCheckCompletedAfterInstant -Checks $Checks -Name 'claude-review' -InstantUtc $HeadCommitCommittedAt
}

function Test-ReviewCategoryAllowsGlobalNoIssueVerdict {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string]$CategoryHeading
    )

    $allowedTerms = @('design', 'architecture', 'api surface')
    $terms = @(
        foreach ($term in [regex]::Split($CategoryHeading, '\s*/\s*')) {
            $normalized = ([regex]::Replace($term.Trim(), '\s+', ' ')).ToLowerInvariant()
            if ($normalized) {
                $normalized
            }
        }
    )

    if ($terms.Count -eq 0) {
        return $false
    }

    foreach ($term in $terms) {
        if ($term -notin $allowedTerms) {
            return $false
        }
    }

    return $true
}

function Get-ActionableReviewBodyReason {
    [CmdletBinding()]
    param(
        [AllowNull()][string]$Body
    )

    if ([string]::IsNullOrWhiteSpace($Body)) {
        return $null
    }

    $verdictMarkers = [regex]::Matches($Body, '(?im)^\s*<!--\s*REVIEW_VERDICT:\s*(CLEAR|BLOCKING)\s*-->\s*$')
    if ($verdictMarkers.Count -gt 0) {
        foreach ($marker in $verdictMarkers) {
            if ($marker.Groups[1].Value -eq 'BLOCKING') {
                return 'review verdict marker: BLOCKING'
            }
        }

        return $null
    }

    $resolvedPriorFindingLineContinuationGuard =
        '(?![^\r\n]*\b(?:fixed|resolved|addressed|verified)\b[^\r\n]*\b(?:but|however|though|except)\b)'
    $resolvedPriorFindingContinuationGuard =
        '(?![\s\S]*\b(?:fixed|resolved|addressed|verified)\b[\s\S]*\b(?:but|however|though|except)\b)'
    $previouslyResolvedHeading =
        '(?=.{1,300}\s*$)previously[- ]flagged\b' +
        $resolvedPriorFindingLineContinuationGuard +
        '(?![^\r\n]*\b(?:not|never|still|un(?:fixed|resolved|addressed|verified|handled))\b)(?=[^\r\n]*\b(?:fixed|resolved|addressed|verified)\b)[^\r\n]*\s*$'
    $resolvedFindingHeading =
        '(?=.{1,300}\s*$)(?![^\r\n]*\b(?:not|never|still|un(?:fixed|resolved|addressed|verified|handled))\b)(?=[^\r\n]*\b(?:fix|fix(?:es|ed)?|change|commit|follow[- ]up|resolves?|resolved|addresses?|addressed|verified)\b)(?=[^\r\n]*\b(?:issues?|bugs?|findings?|concerns?|regressions?)\b)[^\r\n]*\s*$'
    $verifiedCleanHeading =
        '(?=.{1,300}\s*$)merge\s+correctness\b(?=[^\r\n]*\bverified\s+clean\b)[^\r\n]*\s*$'
    $closedCoverageGapHeading =
        '(?=.{1,300}\s*$)test\s+coverage\b[^\r\n]*\bgap\s+closed\b\s*\.?\s*$'
    $nonActionableHeading =
        '(?:\d+\.\s+)?(?:minor|optional|nit|non[- ]blocking)\b' +
        '|not\s+a\s+regression\b(?:,\s+just\s+noting\s+scope)?\s*$' +
        "|$previouslyResolvedHeading" +
        "|$resolvedFindingHeading" +
        "|$verifiedCleanHeading" +
        "|$closedCoverageGapHeading"
    $actionableHeadingWord = 'Correctness|Bug|Bugs|Concern|Concerns|Issue|Issues|Regression|Risk|Risks|Design|Leak|Leaks|Gap|Gaps|Silently|(?<!not )(?<!non-)Blocking|(?<!not )(?<!non-)Blocker|Test coverage gap|Required|Must fix'
    $horizontalWhitespace = '[^\S\r\n]'
    $categoryHeadingTerm = "(?:correctness|security|design|architecture|api$horizontalWhitespace+surface|claude\.md$horizontalWhitespace+(?:compliance|conventions))"
    $categoryOnlyHeading = "$categoryHeadingTerm(?:$horizontalWhitespace*/$horizontalWhitespace*$categoryHeadingTerm)*"
    $noCategoryFindings =
        '\bno\s+(?:\w+\s+){0,5}(?:bugs?|issues?|concerns?|blockers?|findings?|problems?)\b(?:\s+(?:found|detected|identified|seen|remain|remaining))?'
    $globalNoIssueVerdict =
        $Body -match "(?im)^\s*$noCategoryFindings\s*(?:to\s+flag|found|detected|identified|seen|remain|remaining)?\s*\.?(?:\s+(?!but\b|however\b|though\b|except\b).*)?$"
    $positiveVerdictDefect =
        '(?<!not\s)incorrect(?:ly)?|(?<!not\s)(?<!nothing\s)wrong|miss(?:es|ing)?|deadlocks?|will\s+throw|throws?\s+(?:an?\s+)?exception|crash(?:es|ing|ed)?|fixme|nullreferenceexception|box(?:es|ing|ed)?|allocat(?:es|ing|ed)|will\s+allocate|allocate\s+per|(?:per[- ]message|array)\s+(?:\w+\s+){0,3}allocations?|off[- ]by[- ]one|still\s+broken|remains?\s+broken|is\s+broken|leak(?:s|ing|ed)?|never\s+disposed|not\s+disposed|race\s+condition|(?<!under a )race|(?<!not\s)corrupt(?:s|ion|ed|ing)?|vulnerabilit(?:y|ies)|vulnerable|insecure|injection|hardcoded|guessable|session\s+token|expos(?:es|ed|ing)?\s+(?:credentials?|secrets?|tokens?)|stack\s+overflow|hang(?:s|ing)?|forever|infinite\s+loop|use[- ]after[- ]free|double[- ]free|double[- ]charges?|not\s+idempotent|(?<!no\s)data\s+loss|silent(?:ly)?\s+drops?(?:\s+\w+){0,2}\s+messages?|skip(?:s|ped|ping)?\s+validation|design\s+risk|risk\s+(?:for|of)|real\s+concerns?|concerns?\s+about|(?:test\s+)?coverage\s+gap|(?<!no\s)(?<!non[- ])block(?:er|ing)|fix\s+is\s+required|required\s+fix|required\s+before|real\s+(?:bug|issue)|edge\s+case|go(?:es|ing)?\s+stale|stale\s+(?:cache|entry)|never\s+refresh|not\s+(?:thread[- ]safe(?!\s+by\s+design)|safe|correct|fixed|resolved|addressed|scoped)'
    $positiveVerdictBlocker = '\b(?:' + $positiveVerdictDefect + ')\b'
    $positiveVerdictContinuationDefect =
        "$positiveVerdictDefect|(?<!not\s+a\s)(?<!no\s)regressions?(?!\s+(?:coverage|tests?|risk))|now\s+duplicated|(?<!used\s+to\s+be\s)(?<!previously\s)duplicated\s+across|duplicates?\s+logic|duplication\s+of|swallow(?:s|ed|ing)?"
    $positiveVerdictContinuationBlocker =
        '\b(?:' + $positiveVerdictContinuationDefect + ')\b'
    $positiveVerdictAlternatives = @(
        "$noCategoryFindings(?:[\s\S]*)?"
        'looks?\s+(?:right|good)(?:[\s\S]*)?'
        "(?:both\s+)?previously[- ]flagged\b$resolvedPriorFindingContinuationGuard(?![\s\S]*\b(?:not|never|still|un(?:fixed|resolved|addressed|verified|handled))\b)(?=[\s\S]*\b(?:fixed|resolved|addressed|verified)\b)(?:[\s\S]*)?"
        "(?:the\s+)?prior\b$resolvedPriorFindingContinuationGuard(?=[\s\S]*\b(?:findings?|issues?|bugs?)\b)(?=[\s\S]*\b(?:fixed|resolved|addressed|verified)\b)(?:[\s\S]*)?"
        'verified(?:\s+against\b[\s\S]*)?'
        'confirmed\b(?:[\s\S]*)?'
        'no\s+concerns\b(?:[\s\S]*)?'
        '`?configureawait\(false\)`?(?:[\s\S]*\b)?(?:used|applied)\s+consistently(?:[\s\S]*)?'
        '(?:the\s+)?core\s+fix\s+is\s+(?:sound|correct)(?:[\s\S]*)?'
        'genuine\s+improvement,\s+not\s+just\s+churn(?:[\s\S]*)?'
        'fix\s+is\s+scoped(?:\s+to\b[\s\S]*)?'
        '(?:[\s\S]*\b)?allocation[- ]free(?:[\s\S]*)?'
        '(?:[\s\S]*\b)?verified\s+no\s+behavior\s+change(?:[\s\S]*)?'
    ) -join '|'
    $positiveCategoryVerdict =
        "(?is)^(?!.*$positiveVerdictBlocker)(?!.*$positiveVerdictContinuationBlocker)\s*(?:[-*]\s*)?(?:$positiveVerdictAlternatives)\.?\s*$"
    $positiveCategoryHeadingVerdict = $positiveCategoryVerdict
    $positiveCategorySection = $positiveCategoryVerdict
    $positiveVerdictLineAlternatives = @(
        "$noCategoryFindings[^\r\n]*"
        "looks?$horizontalWhitespace+(?:right|good)[^\r\n]*"
        "verified(?:$horizontalWhitespace+against\b[^\r\n]*)?"
        'confirmed\b[^\r\n]*'
        "no$horizontalWhitespace+concerns\b[^\r\n]*"
        "(?:the$horizontalWhitespace+)?core$horizontalWhitespace+fix$horizontalWhitespace+is$horizontalWhitespace+(?:sound|correct)\b[^\r\n]*"
        "fix$horizontalWhitespace+is$horizontalWhitespace+scoped(?:$horizontalWhitespace+to\b[^\r\n]*)?"
    ) -join '|'
    $positiveCategoryVerdictLine =
        "(?im)^$horizontalWhitespace*(?:[-*]$horizontalWhitespace*)?(?![^\r\n]*\b(?:but|however|though|except)\b)(?:$positiveVerdictLineAlternatives)\.?$horizontalWhitespace*\r?$"

    $categoryHeadingWithOptionalVerdict = "$categoryOnlyHeading(?:(?:$horizontalWhitespace*(?:[-:]|\p{Pd})$horizontalWhitespace*\S[^\r\n#]*)|(?:$horizontalWhitespace*\([^\r\n#)]*\))|(?:$horizontalWhitespace+\S[^\r\n#]*))?:?"
    $categoryHeadingPattern = "(?im)^$horizontalWhitespace*#{2,4}$horizontalWhitespace+($categoryOnlyHeading)(?:(?:$horizontalWhitespace*(?:[-:]|\p{Pd})$horizontalWhitespace*(?<verdict>\S[^\r\n#]*))|(?:$horizontalWhitespace*\((?<parentheticalVerdict>[^)\r\n#]+)\))|(?:$horizontalWhitespace+(?<bareVerdict>\S[^\r\n#]*)))?:?$horizontalWhitespace*\r?$"
    foreach ($heading in [regex]::Matches($Body, $categoryHeadingPattern)) {
        $allowsGlobalNoIssueVerdict = Test-ReviewCategoryAllowsGlobalNoIssueVerdict -CategoryHeading $heading.Groups[1].Value
        $headingVerdict = if ($heading.Groups['verdict'].Success) {
            $heading.Groups['verdict'].Value
        } elseif ($heading.Groups['bareVerdict'].Success) {
            $heading.Groups['bareVerdict'].Value
        } else {
            $heading.Groups['parentheticalVerdict'].Value
        }
        $headingVerdictResolvesPriorFindings = $headingVerdict -match "(?is)^(?!.*$positiveVerdictBlocker)(?!.*$positiveVerdictContinuationBlocker)\s*(?:both\s+)?previously[- ]flagged\b$resolvedPriorFindingContinuationGuard(?![\s\S]*\b(?:not|never|still|un(?:fixed|resolved|addressed|verified|handled))\b)(?=[\s\S]*\b(?:fixed|resolved|addressed|verified)\b)(?:[\s\S]*)?$"
        if ($headingVerdict -and $headingVerdict -notmatch $positiveCategoryHeadingVerdict) {
            return "actionable category heading: $($heading.Value.Trim())"
        }

        $sectionStart = $heading.Index + $heading.Length
        $sectionRemainder = $Body.Substring($sectionStart)
        $nextHeading = [regex]::Match($sectionRemainder, '(?m)^\s*#{2,4}\s+')
        $sectionBody = if ($nextHeading.Success) {
            $sectionRemainder.Substring(0, $nextHeading.Index)
        } else {
            $sectionRemainder
        }
        $firstVerdictMatch = [regex]::Match($sectionBody, '(?m)\S[^\r\n]*')
        $firstVerdict = $firstVerdictMatch.Value.Trim()
        $sectionHasPositiveVerdictLine = $sectionBody -match $positiveCategoryVerdictLine
        $sectionStartsWithTraceAndAllClear = $firstVerdict -match "(?i)^\s*(?:[-*]\s*)?traced\b" -and $sectionHasPositiveVerdictLine

        if ([string]::IsNullOrWhiteSpace($firstVerdict)) {
            if ($headingVerdict) {
                continue
            }

            return "actionable category heading: $($heading.Value.Trim())"
        }

        if ($firstVerdict -notmatch $positiveCategorySection -and
            -not ($headingVerdictResolvesPriorFindings -and $sectionBody -notmatch $positiveVerdictContinuationBlocker) -and
            -not ($sectionStartsWithTraceAndAllClear -and $sectionBody -notmatch $positiveVerdictContinuationBlocker) -and
            -not ($allowsGlobalNoIssueVerdict -and $globalNoIssueVerdict -and $sectionBody -notmatch $positiveVerdictContinuationBlocker)) {
            return "actionable category heading: $($heading.Value.Trim())"
        }

        $sectionTailStart = $firstVerdictMatch.Index + $firstVerdictMatch.Length
        $sectionTail = $sectionBody.Substring($sectionTailStart)
        if ($sectionTail -match $positiveVerdictContinuationBlocker) {
            return "actionable category heading: $($heading.Value.Trim())"
        }
    }

    $patterns = @(
        @{
            Reason = 'actionable heading'
            Pattern = "(?im)^\s*#{2,4}\s+(?!$nonActionableHeading)(?!$categoryHeadingWithOptionalVerdict\s*$).*\b($actionableHeadingWord)\b"
        },
        @{
            Reason = 'numbered finding heading'
            Pattern = '(?im)^\s*#{2,4}\s+\d+\.\s+(?!(?:minor|optional|nit|non[- ]blocking)\b).+'
        },
        @{
            Reason = 'before-merge action'
            Pattern = '(?im)\b(?:worth|needs?|should|must|required|please|recommend(?:ed)?)\s+(?:be\s+)?(?:address(?:ed|ing)?|fix(?:ed|ing)?)\b[^\r\n.]{0,120}\b(?:before\s+merg(?:e|ing)|prior\s+to\s+merg(?:e|ing))\b|\b(?:before\s+merg(?:e|ing)|prior\s+to\s+merg(?:e|ing))\b[^\r\n.]{0,120}\b(?:worth|needs?|should|must|required|please|recommend(?:ed)?)\s+(?:be\s+)?(?:address(?:ed|ing)?|fix(?:ed|ing)?)\b'
        }
    )

    foreach ($entry in $patterns) {
        if ($Body -match $entry.Pattern) {
            return "$($entry.Reason): $($Matches[0].Trim())"
        }
    }

    return $null
}
