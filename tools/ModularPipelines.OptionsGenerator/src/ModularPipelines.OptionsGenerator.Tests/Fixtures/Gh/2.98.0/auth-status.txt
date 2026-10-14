Display active account and authentication state on each known GitHub host.

For each host, the authentication state of each known account is tested and any issues are included in the output.
Each host section will indicate the active account, which will be used when targeting that host.

If an account on any host (or only the one given via `--hostname`) has authentication issues,
the command will exit with 1 and output to stderr. Note that when using the `--json` option, the command
will always exit with zero regardless of any authentication issues, unless there is a fatal error.

To change the active account for a host, see `gh auth switch`.

For more information about output formatting flags, see `gh help formatting`.

USAGE
  gh auth status [flags]

FLAGS
  -a, --active            Display the active account only
  -h, --hostname string   Check only a specific hostname's auth status
      --jq expression     Filter JSON output using a jq expression
      --json fields       Output JSON with the specified fields
  -t, --show-token        Display the auth token
      --template string   Format JSON output using a Go template; see "gh help formatting"

INHERITED FLAGS
  --help   Show help for command

JSON FIELDS
  hosts

EXAMPLES
  # Display authentication status for all accounts on all hosts
  $ gh auth status

  # Display authentication status for the active account on a specific host
  $ gh auth status --active --hostname github.example.com

  # Display tokens in plain text
  $ gh auth status --show-token

  # Format authentication status as JSON
  $ gh auth status --json hosts

  # Include plain text token in JSON output
  $ gh auth status --json hosts --show-token

  # Format hosts as a flat JSON array
  $ gh auth status --json hosts --jq '.hosts | add'

LEARN MORE
  Use `gh <command> <subcommand> --help` for more information about a command.
  Read the manual at https://cli.github.com/manual
  Learn about exit codes using `gh help exit-codes`
  Learn about accessibility experiences using `gh help accessibility`
