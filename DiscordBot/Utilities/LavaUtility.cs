using DSharpPlus.CommandsNext;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Utilities
{
  public static class LavaUtility
  {
    public static (LavalinkExtension lava, 
      LavalinkNodeConnection node, 
      LavalinkGuildConnection guildConnection)
      GetLavaConnections(CommandContext context)
    {
      var lava = context.Client.GetLavalink();
      var node = lava.ConnectedNodes.Values.First();
      var guildConnection = node.GetGuildConnection(context.Member.VoiceState.Guild);
      return (lava, node, guildConnection);
    }

    public static async Task StartLavalink()
    {
      string path = Directory.GetCurrentDirectory();
      path += "\\Lavalink.jar";
      var lavaLink = new Process();
      lavaLink.StartInfo.UseShellExecute = false;
      lavaLink.StartInfo.FileName = "java";
      lavaLink.StartInfo.Arguments = $"-jar {path}";
      lavaLink.Start();

      // Wait for Lavalink to start.
      await Task.Delay(10000);
    }

    public static async Task ConnectLavaNode(LavalinkExtension lava)
    {
      var lavaEndpoint = new ConnectionEndpoint
      {
        Hostname = "127.0.0.1",
        Port = 2333
      };

      var lavalinkConfig = new LavalinkConfiguration
      {
        Password = "youshallnotpass",
        RestEndpoint = lavaEndpoint,
        SocketEndpoint = lavaEndpoint
      };
      await lava.ConnectAsync(lavalinkConfig);
    }
  }
}
