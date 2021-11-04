using DSharpPlus.CommandsNext;
using DSharpPlus.Lavalink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
  }
}
