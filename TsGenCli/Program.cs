using Spectre.Console.Cli;
using TsGenCli.Commands;

var app = new CommandApp<GenerateCommand>();

return await app.RunAsync(args);
