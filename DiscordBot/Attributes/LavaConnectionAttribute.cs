using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Lavalink;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Attributes
{
  class LavaConnectionAttribute : CheckBaseAttribute
  {
    public override async Task<bool> ExecuteCheckAsync(CommandContext context, bool help)
    {
      var lava = context.Client.GetLavalink();
      if (!lava.ConnectedNodes.Any())
      {
        await context.RespondAsync("Attemping to start Lavalink...");
        await Program.ConnectLavaNode(lava);
        if (!lava.ConnectedNodes.Any())
        {
          await context.RespondAsync("Lavalink failed to start.");
          return false;
        }  
      }

      var node = lava.ConnectedNodes.Values.First();

      await node.ConnectAsync(context.Member.VoiceState.Channel);
      return true;
    }
  }
}
