# TS Gen

A typescript generator dotnet tool.

Annotate your classes with the TsGen attribute, install the CLI (TsGenCli) as a .NET tool, and run tsgen to build types.

Include a class extending TsGenSettings to customize the output path and more.

Docs can be found at [https://adv76.github.io/TsGen/](https://adv76.github.io/TsGen/).

NOTICE: This package is in beta. Internal APIs can change at any time as development is ongoing. If only using with the TsGenCli, there should be little to no changes or issues.

## BREAKING CHANGES

In version 1.0.0-alpha.9, The Property "OutputDirectory" in the TsGenSettings class was replaced with a string[] OutputDirectories to allow multi-directory output
