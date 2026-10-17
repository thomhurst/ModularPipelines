# Build image for the Anthropic OSS Scanner (https://github.com/anthropics/oss-scanner).
# The scanner builds this file with the repository as the build context, then removes network access,
# so every SDK, NuGet package and test dependency must be fetched here.
FROM debian:trixie

ENV DEBIAN_FRONTEND=noninteractive \
    DOTNET_ROOT=/usr/share/dotnet \
    DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_NOLOGO=1 \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1 \
    NUGET_PACKAGES=/root/.nuget/packages
ENV PATH="${DOTNET_ROOT}:/root/.dotnet/tools:${PATH}"

RUN apt-get update && apt-get install -y --no-install-recommends \
      ca-certificates curl git procps unzip zip \
      libc6 libgcc-s1 libicu72 libssl3 libstdc++6 zlib1g libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

# Many command, logging and secret-masking tests run PowerShell scripts.
RUN curl -fsSL https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -o /tmp/packages-microsoft-prod.deb \
    && dpkg -i /tmp/packages-microsoft-prod.deb \
    && rm /tmp/packages-microsoft-prod.deb \
    && apt-get update && apt-get install -y --no-install-recommends powershell \
    && rm -rf /var/lib/apt/lists/*

# scanner contract: the checkout lives inside the image, at /src
COPY . /src
WORKDIR /src

# Install the exact SDK pinned in global.json (currently a .NET 11 release candidate).
RUN curl -fsSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh \
    && bash /tmp/dotnet-install.sh --jsonfile /src/global.json --install-dir "${DOTNET_ROOT}" \
    && rm /tmp/dotnet-install.sh \
    && dotnet --info

# Core library, analyzers, source generator, and the hand-written packages with the most attack surface.
# The ~60 generated CLI integrations (src/ModularPipelines.<Tool>) are thin option wrappers over the core
# argument builder and are not built here. They depend only on the core packages restored below, so one can
# usually be built offline with:
#   dotnet build src/ModularPipelines.<Tool>/ModularPipelines.<Tool>.slnx
# Do not build ModularPipelines.All.slnx: it is very large and is not needed for analysis.
ARG TEST_PROJECTS="\
 test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj \
 test/ModularPipelines.Distributed.UnitTests/ModularPipelines.Distributed.UnitTests.csproj \
 test/ModularPipelines.Distributed.SignalR.UnitTests/ModularPipelines.Distributed.SignalR.UnitTests.csproj \
 test/ModularPipelines.Distributed.Redis.UnitTests/ModularPipelines.Distributed.Redis.UnitTests.csproj \
 test/ModularPipelines.Distributed.Discovery.Redis.UnitTests/ModularPipelines.Distributed.Discovery.Redis.UnitTests.csproj \
 test/ModularPipelines.Distributed.Artifacts.S3.UnitTests/ModularPipelines.Distributed.Artifacts.S3.UnitTests.csproj \
 test/ModularPipelines.Email.UnitTests/ModularPipelines.Email.UnitTests.csproj \
 test/ModularPipelines.Ftp.UnitTests/ModularPipelines.Ftp.UnitTests.csproj \
 test/ModularPipelines.Git.UnitTests/ModularPipelines.Git.UnitTests.csproj \
 test/ModularPipelines.GitHub.UnitTests/ModularPipelines.GitHub.UnitTests.csproj"

# Restore while online, then build with debug symbols. A failing build fails the image.
RUN dotnet build ModularPipelines.slnx -c Debug \
    && for project in ${TEST_PROJECTS}; do dotnet build "$project" -c Debug || exit 1; done

# Smoke-run the core unit tests so the image is known to work; failures do not abort the build.
# Some tests need external tools or network services that are absent in the scanner and will fail or skip.
RUN timeout 1200 dotnet run --project test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj \
      -c Debug --framework net11.0 --no-build \
    || echo "WARNING: core unit tests reported failures"
