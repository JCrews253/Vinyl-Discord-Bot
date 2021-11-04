using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Attributes
{
  class VoiceConnectionAttribute : CheckBaseAttribute
  {
    public override async Task<bool> ExecuteCheckAsync(CommandContext context, bool help)
    {
      if (context.Member.VoiceState == null || context.Member.VoiceState.Channel == null)
      {
        await context.RespondAsync("You are not in a voice channel.");
        return false;
      }
      else
      {
        return true;
      }
    }
  }
}
