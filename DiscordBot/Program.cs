using DSharpPlus;
using System;
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
      var discord = new DiscordClient(new DiscordConfiguration()
      {
        Token = "NzczNzIxMTI1ODcxODEyNjIy.X6NV8w.G20JhtiqCbYeiVKNQ8yk6aei0aY",
        TokenType = TokenType.Bot,
        Intents = DiscordIntents.AllUnprivileged
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
