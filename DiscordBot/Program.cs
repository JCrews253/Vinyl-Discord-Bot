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

namespace DiscordBot
{
  public class Program
  {
    private static Config _config;

    static async Task Main(string[] args)
    {
      await LavaUtility.StartLavalink();

      // Read config file
      using(var fs = File.OpenRead(@"Configs\config.json"))
      using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
      {
        var json = await sr.ReadToEndAsync();
        _config = JsonConvert.DeserializeObject<Config>(json);
      }

      // Create discord client
      var client = new DiscordClient(new DiscordConfiguration()
      {
        Token = _config.Token,
        TokenType = TokenType.Bot,
        AutoReconnect = true,
        Intents = DiscordIntents.AllUnprivileged    
      });

      var commmandConfig = new CommandsNextConfiguration
      {
        StringPrefixes = new[] { _config.Prefix }
      };
      var commands = client.UseCommandsNext(commmandConfig);

      var lava = client.UseLavalink();

      commands.RegisterCommands<PingCommand>();
      commands.RegisterCommands<MusicCommands>();
      commands.RegisterCommands<ChatCommands>();

      client.Ready += SetStatus;
      await client.ConnectAsync();
      await LavaUtility.ConnectLavaNode(lava);
      await Task.Delay(-1);
    }

    static async Task SetStatus(DiscordClient client, ReadyEventArgs e)
    {
      await client.UpdateStatusAsync(activity: new DiscordActivity(".help"));
    }
  }
}
