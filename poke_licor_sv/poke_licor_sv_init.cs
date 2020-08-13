using CitizenFX.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace poke_licor_sv
{
    public class poke_licor_sv_init : BaseScript
    {
        public poke_licor_sv_init()
        {
            initRegisterItems();
        }

        private async Task initRegisterItems()
        {
            await Delay(4000);
            for (int i = 0; i < LoadConfig.Config["ItemsToUse"].Count(); i++)
            {
                await Delay(200);
                int index = i;
                TriggerEvent("vorpCore:registerUsableItem", LoadConfig.Config["ItemsToUse"][i]["Name"].ToString(), new Action<dynamic>((data) =>
                {
                    Player source = getSource(data.source);
                    TriggerEvent("vorpCore:subItem", data.source, LoadConfig.Config["ItemsToUse"][index]["Name"].ToString(), 1);
                    source.TriggerEvent("poke_licor:useItem", LoadConfig.Config["ItemsToUse"][index]["Label"].ToString(), LoadConfig.Config["ItemsToUse"][index]["CoreType"].ToObject<int>(), LoadConfig.Config["ItemsToUse"][index]["ObjectModel"].ToString(), LoadConfig.Config["ItemsToUse"][index]["propId"].ToString(), LoadConfig.Config["ItemsToUse"][index]["itemInteraction"].ToString(), LoadConfig.Config["ItemsToUse"][index]["AnimFxType"].ToString());
                }));
            }
        }

        public static Player getSource(int handle)
        {
            PlayerList pl = new PlayerList();
            Player p = pl[handle];
            return p;
        }
    }
}
