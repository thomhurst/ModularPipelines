List issues in a GitHub repository. By default, this only lists open issues.

The search query syntax is documented here:
<https://docs.github.com/en/search-github/searching-on-github/searching-issues-and-pull-requests>

On supported GitHub hosts, advanced issue search syntax can be used in the
`--search` query. For more information about advanced issue search, see:
<https://docs.github.com/en/issues/tracking-your-work-with-issues/using-issues/filtering-and-searching-issues-and-pull-requests#building-advanced-filters-for-issues>

For more information about output formatting flags, see `gh help formatting`.

USAGE
  gh issue list [flags]

ALIASES
  gh issue ls

FLAGS
      --app string         Filter by GitHub App author
  -a, --assignee string    Filter by assignee
  -A, --author string      Filter by author (use --app to filter by a GitHub App)
  -q, --jq expression      Filter JSON output using a jq expression
      --json fields        Output JSON with the specified fields
  -l, --label strings      Filter by label
  -L, --limit int          Maximum number of issues to fetch (default 30)
      --mention string     Filter by mention
  -m, --milestone string   Filter by milestone number or title
  -S, --search query       Search issues with query
  -s, --state string       Filter by state: {open|closed|all} (default "open")
  -t, --template string    Format JSON output using a Go template; see "gh help formatting"
      --type name          Filter by issue type name
  -w, --web                List issues in the web browser

INHERITED FLAGS
      --help                     Show help for command
  -R, --repo [HOST/]OWNER/REPO   Select another repository using the [HOST/]OWNER/REPO format

JSON FIELDS
  assignees, author, blockedBy, blocking, body, closed, closedAt,
  closedByPullRequestsReferences, comments, createdAt, id, isPinned, issueType,
  labels, milestone, number, parent, projectCards, projectItems, reactionGroups,
  state, stateReason, subIssues, subIssuesSummary, title, updatedAt, url

EXAMPLES
  $ gh issue list --label "bug" --label "help wanted"
  $ gh issue list --author monalisa
  $ gh issue list --app dependabot
  $ gh issue list --assignee "@me"
  $ gh issue list --milestone "The big 1.0"
  $ gh issue list --search "error no:assignee sort:created-asc"
  $ gh issue list --state all
  $ gh issue list --type Bug

LEARN MORE
  Use `gh <command> <subcommand> --help` for more information about a command.
  Read the manual at https://cli.github.com/manual
  Learn about exit codes using `gh help exit-codes`
  Learn about accessibility experiences using `gh help accessibility`
