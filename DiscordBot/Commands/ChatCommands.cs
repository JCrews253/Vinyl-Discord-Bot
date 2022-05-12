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
    [Description("Clears all messages from bots and messages start with the set command prefix.")]
    public async Task Clean(CommandContext context)
    {
      var messages = await context.Channel.GetMessagesAsync(limit: 100);
      List<DiscordMessage> messagesToDelete = new List<DiscordMessage>();
      foreach (var message in messages)
      {
        if (message.Content.StartsWith(".") || message.Content.StartsWith("?.") || message.Author.IsBot)
        {
          messagesToDelete.Add(message);
        }
      };

      await context.Channel.DeleteMessagesAsync(messagesToDelete);
    }

    [Command]
    [Aliases("VinylInviteLink","invite")]
    [Description("Url to add Vinyl to your discord server.")]
    public async Task BotInviteLink(CommandContext context)
    {
      await context.RespondAsync("https://discord.com/oauth2/authorize?client_id=773721125871812622&scope=bot");
    }
  }
}
