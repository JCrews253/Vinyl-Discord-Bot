using DiscordBot.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Lavalink;
using DiscordBot.Configs;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DiscordBot.Utilities;
using System.Diagnostics;
using Vinyl.Commands;

namespace DiscordBot
{
  public class Program
  {
    private static Config _config;

    static async Task Main(string[] args)
    {
      await LavaUtility.StartLavalink();

      // Read config file
      using(var fs = File.OpenRead(@"Configs/config.json"))
      using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
      {
        var json = await sr.ReadToEndAsync();
        _config = JsonConvert.DeserializeObject<Config>(json);
      }

      // Create discord client
      var client = new DiscordClient(new DiscordConfiguration()
      {
        Token = Debugger.IsAttached ? _config.DevToken : _config.Token,
        TokenType = TokenType.Bot,
        AutoReconnect = true,
        Intents = DiscordIntents.AllUnprivileged    
      });

      var commmandConfig = new CommandsNextConfiguration
      {
        StringPrefixes = new[] { Debugger.IsAttached ? _config.DevPrefix : _config.Prefix }
      };
      var commands = client.UseCommandsNext(commmandConfig);

      var lava = client.UseLavalink();

      commands.RegisterCommands<PingCommand>();
      commands.RegisterCommands<MusicCommands>();
      commands.RegisterCommands<ChatCommands>();
      commands.RegisterCommands<PlexCommands>();

      client.Ready += SetStatus;
      await client.ConnectAsync();
      await LavaUtility.ConnectLavaNode(lava);
      await Task.Delay(-1);
    }

    static async Task SetStatus(DiscordClient client, ReadyEventArgs e)
    {
      var prefix = Debugger.IsAttached ? _config.DevPrefix : _config.Prefix;
      await client.UpdateStatusAsync(activity: new DiscordActivity($"{prefix}help"));
    }
  }
}
