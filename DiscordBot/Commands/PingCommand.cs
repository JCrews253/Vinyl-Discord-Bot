using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
  public class PingCommand : BaseCommandModule
  {
    [Command("ping")]
    [Aliases("pong")]
    [Description("Example ping command")]
    public async Task Ping(CommandContext context)
    {
      await context.TriggerTypingAsync();

      var emoji = DiscordEmoji.FromName(context.Client, ":ping_pong:");

      await context.RespondAsync($"{emoji} Pong! Ping: {context.Client.Ping}ms");
    }
  }
}
