using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Threading.Tasks;

namespace poke_licor_cl
{
    public class poke_licor_cl_init : BaseScript
    {

        private bool drink = false;
        private bool drunk = false;
        int WhiskeyDrank = 0;
        int timer = 0;
        string effect = "";
        public poke_licor_cl_init()
        {
            EventHandlers["poke_licor:useItem"] += new Action<string, int, string, string, string, string>(item_UseItem);
            Tick += OnTick;
        }

        private async void item_UseItem(string item, int coretype, string objectModel, string propId, string itemInteraction, string animFxType)
        {

            Vector3 coords = API.GetEntityCoords(API.PlayerPedId(), true, true);
            uint itemHash = (uint)API.GetHashKey(objectModel);
            int objeto = API.CreateObject(itemHash, coords.X, coords.Y, coords.Z, false, true, false, false, true);
            API.TaskItemInteraction_2(API.PlayerPedId(), -1199896558, objeto, API.GetHashKey(propId), API.GetHashKey(itemInteraction), 1, 0, -1);
            drink = true;
            while (drink)
            {
                await Delay(1);
                bool retval = API.DoesEntityExist(objeto);
                if (!retval)
                {
                    drink = false;
                    WhiskeyDrank = WhiskeyDrank += 1;
                    if (WhiskeyDrank >= 1 && (!drunk))
                    {
                        timer = Convert.ToInt32(GetConfig.Config["drunkTime"]);
                        drunk = true;
                        effect = animFxType;
                        Function.Call((Hash)0x4102732DF6B4005F, animFxType, 0, true);
                        if (coretype != -1)
                        {
                            Function.Call((Hash)0xC6258F41D86676E0, API.PlayerPedId(), coretype, 100);
                            Function.Call((Hash)0x4AF5A4C7B9157D14, API.PlayerPedId(), coretype, 2000.0);
                            Function.Call((Hash)0xF6A7C08DF2E28B28, API.PlayerPedId(), 0, 2000.0);
                        }
                        Function.Call((Hash)0x406CCF555B04FAD3, API.PlayerPedId(), 1, 1.0f);
                        TriggerEvent("vorp:Tip", GetConfig.Langs["you_consumed"] + item, 3000);
                    }
                }
            }
        }

        private async Task OnTick()
        {
            await Delay(1000);
            if (drunk)
            {
                if (timer > 0)
                {
                    timer = timer -= 1;
                }
                else
                {
                    Function.Call((Hash)0x406CCF555B04FAD3, API.PlayerPedId(), 1, 0.0f);
                    drunk = false;
                    WhiskeyDrank = 0;
                    if (Function.Call<bool>((Hash)0x4A123E85D7C4CA0B, effect))
                    {
                        Function.Call((Hash)0xB4FD7446BAB2F394, effect);
                    }
                }
            }
        }
    }
}
