using CBGames.Core;
using CBGames.Player;
using Invector;
using UnityEngine;

namespace CBGames.Objects
{
    public class MP_Projectile : MonoBehaviour
    {
        public void TeamDamageCheck(vDamage damage)
        {
            if (damage.receiver.GetComponentInParent<SyncPlayer>() && 
                damage.receiver.GetComponentInParent<SyncPlayer>().teamName == damage.sender.GetComponentInParent<SyncPlayer>().teamName &&
                NetworkManager.networkManager.allowTeamDamaging == false)
            {
                damage.hitReaction = false;
                damage.activeRagdoll = false;
                damage.damageValue = 0;
                damage.ReduceDamage(100);
            }
        }
    }
}