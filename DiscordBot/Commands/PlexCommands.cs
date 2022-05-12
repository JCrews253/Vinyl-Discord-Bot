using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Vinyl.Commands
{
  public class PlexCommands : BaseCommandModule
  {
    [Command]
    [Aliases("rplex")]
    [Description("Restart Plex server if it is down.")]
    public async Task RestartPlex(CommandContext context)
    {
      var plexProcesses = Process.GetProcessesByName("Plex Media Server");
      if(plexProcesses.Length > 0)
      {
        await context.RespondAsync($"Plex is already running.");
      }
      else
      {
        await context.RespondAsync($"Starting Plex...");
        var process = Process.Start(@"C:\Program Files (x86)\Plex\Plex Media Server\Plex Media Server.exe");
        await context.RespondAsync($"Plex server is now running with id: {process.Id}");
      }
    }
  }
}
