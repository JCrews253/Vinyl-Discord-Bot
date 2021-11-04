using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
  public class ChatCommands : BaseCommandModule
  {
    [Command]
    public async Task Clean(CommandContext context)
    {
      var messages = await context.Channel.GetMessagesAsync(limit: 100);
      List<DiscordMessage> messagesToDelete = new List<DiscordMessage>();
      foreach (var message in messages)
      {
        if (message.Content.StartsWith(".") || message.Author.IsBot)
        {
          messagesToDelete.Add(message);
        }
      };

      await context.Channel.DeleteMessagesAsync(messagesToDelete);
    }
  }
}
