using DiscordBot.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Net;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Lavalink;
using DiscordBot.Configs;

namespace DiscordBot
{
  public class Program
  {
    private static Config _config;
    private static DiscordClient _client;
    private static CommandsNextExtension _commands;

    static void Main(string[] args)
    {
      MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
      // Read config file
      using(var fs = File.OpenRead(@"Configs\config.json"))
      using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
      {
        var json = await sr.ReadToEndAsync();
        _config = JsonConvert.DeserializeObject<Config>(json);
      }

      // Create discord client
      _client = new DiscordClient(new DiscordConfiguration()
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
      _commands = _client.UseCommandsNext(commmandConfig);

      var endpoint = new ConnectionEndpoint
      {
        Hostname = "127.0.0.1", 
        Port = 2333 
      };

      var lavalinkConfig = new DSharpPlus.Lavalink.LavalinkConfiguration
      {
        Password = "youshallnotpass",
        RestEndpoint = endpoint,
        SocketEndpoint = endpoint
      };

      var lavalink = _client.UseLavalink();

      RegisterCommands();

      await _client.ConnectAsync();
      await lavalink.ConnectAsync(lavalinkConfig);
      await Task.Delay(-1);
    }

    static void RegisterCommands()
    {
      _commands.RegisterCommands<PingCommand>();
      _commands.RegisterCommands<MusicCommands>();
      _commands.RegisterCommands<ChatCommands>();
    }
  }
}
