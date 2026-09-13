Create a pull request on GitHub.

Upon success, the URL of the created pull request will be printed.

When the current branch isn't fully pushed to a git remote, a prompt will ask where
to push the branch and offer an option to fork the base repository. Any fork created this
way will only have the default branch of the upstream repository. Use `--head` to
explicitly skip any forking or pushing behavior.

`--head` supports `<user>:<branch>` syntax to select a head repo owned by `<user>`.
Using an organization as the `<user>` is currently not supported.
For more information, see <https://github.com/cli/cli/issues/10093>

A prompt will also ask for the title and the body of the pull request. Use `--title` and
`--body` to skip this, or use `--fill` to autofill these values from git commits.
It's important to notice that if the `--title` and/or `--body` are also provided
alongside `--fill`, the values specified by `--title` and/or `--body` will
take precedence and overwrite any autofilled content.

The base branch for the created PR can be specified using the `--base` flag. If not provided,
the value of `gh-merge-base` git branch config will be used. If not configured, the repository's
default branch will be used. Run `git config branch.{current}.gh-merge-base {base}` to configure
the current branch to use the specified merge base.

Link an issue to the pull request by referencing the issue in the body of the pull
request. If the body text mentions `Fixes #123` or `Closes #123`, the referenced issue
will automatically get closed when the pull request gets merged.

Use `--attach` to upload an image or video. The attachment is appended to the
body. If the body references an attached file, such as `![alt](./login.png)`, that
reference is rewritten to point at the uploaded asset instead.
You can attach up to 50 files per command.

Alt text for an image follows the path after `#`, as in
`--attach './login.png#The login error state'`. Without it the filename is used.
A reference already in the body keeps the alt text written there. Video renders
as a player and has no alt text, so it cannot be given any.

If some attachments upload and others fail, the pull request is still created with the
ones that succeeded. The command then exits with a non-zero status, but the new pull
request's URL is still printed to stdout.

By default, users with write access to the base repository can push new commits to the
head branch of the pull request. Disable this with `--no-maintainer-edit`.

Adding a pull request to projects requires authorization with the `project` scope.
To authorize, run `gh auth refresh -s project`.


USAGE
  gh pr create [flags]

ALIASES
  gh pr new

FLAGS
  -a, --assignee login       Assign people by their login. Use "@me" to self-assign.
      --attach file          Attach an image or video file, in '<file>#<image alt text>' format
  -B, --base branch          The branch into which you want your code merged
  -b, --body string          Body for the pull request
  -F, --body-file file       Read body text from file (use "-" to read from standard input)
  -d, --draft                Mark pull request as a draft
      --dry-run              Print details instead of creating the PR. May still push git changes.
  -e, --editor               Skip prompts and open the text editor to write the title and body in. The first line is the title and the remaining text is the body.
  -f, --fill                 Use commit info for title and body
      --fill-first           Use first commit info for title and body
      --fill-verbose         Use commits msg+body for description
  -H, --head branch          The branch that contains commits for your pull request (default [current branch])
  -l, --label name           Add labels by name
  -m, --milestone name       Add the pull request to a milestone by name
      --no-maintainer-edit   Disable maintainer's ability to modify pull request
  -p, --project title        Add the pull request to projects by title
      --recover string       Recover input from a failed run of create
  -r, --reviewer handle      Request reviews from people or teams by their handle
  -T, --template file        Template file to use as starting body text
  -t, --title string         Title for the pull request
  -w, --web                  Open the web browser to create a pull request

INHERITED FLAGS
      --help                     Show help for command
  -R, --repo [HOST/]OWNER/REPO   Select another repository using the [HOST/]OWNER/REPO format

EXAMPLES
  $ gh pr create --title "The bug is fixed" --body "Everything works again"
  $ gh pr create --reviewer monalisa,hubot  --reviewer myorg/team-name
  $ gh pr create --project "Roadmap"
  $ gh pr create --base develop --head monalisa:feature
  $ gh pr create --template "pull_request_template.md"
  $ gh pr create --attach './login.png#The login error state'
  $ gh pr create --attach ./before.png --attach ./after.png

LEARN MORE
  Use `gh <command> <subcommand> --help` for more information about a command.
  Read the manual at https://cli.github.com/manual
  Learn about exit codes using `gh help exit-codes`
  Learn about accessibility experiences using `gh help accessibility`
