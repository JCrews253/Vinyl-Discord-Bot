using DSharpPlus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
  class Program
  {
    static void Main(string[] args)
    {
      MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
      var json = string.Empty;

      using(var fs = File.OpenRead("config.json"))
      using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
      {
        json = await sr.ReadToEndAsync();
      }

      var config = JsonConvert.DeserializeObject<Config>(json);

      var discord = new DiscordClient(new DiscordConfiguration()
      {
        Token = config.Token,
        TokenType = TokenType.Bot,
        AutoReconnect = true,
        Intents = DiscordIntents.AllUnprivileged,
        
      });

      discord.MessageCreated += async (s, e) =>
      {
        if (e.Message.Content.ToLower().StartsWith("ping"))
          await e.Message.RespondAsync("pong!");

      };

      await discord.ConnectAsync();
      await Task.Delay(-1);
    }
  }
}
