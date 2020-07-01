using Invector.vMelee;
using UnityEngine;
namespace Invector.vItemManager
{
    [vClassHeader("Melee Equipment", openClose = false, useHelpBox = true, helpBoxText = "Use this component if you also use the ItemManager in your Character")]
    public class vMeleeEquipment : vEquipment
    {
        vMeleeWeapon _weapon;
        bool withoutWeapon;

        protected virtual vMeleeWeapon weapon
        {
            get
            {
                if (!_weapon && !withoutWeapon)
                {
                    _weapon = GetComponent<vMeleeWeapon>();
                    if (!_weapon) withoutWeapon = true;
                }

                return _weapon;
            }
        }

        public override void OnEquip(vItem item)
        {
            if (!weapon) return;
            base.OnEquip(item);
            var damage = item.GetItemAttribute(vItemAttributes.Damage);
            var staminaCost = item.GetItemAttribute(vItemAttributes.StaminaCost);
            var defenseRate = item.GetItemAttribute(vItemAttributes.DefenseRate);
            var defenseRange = item.GetItemAttribute(vItemAttributes.DefenseRange);
            if (damage != null) this.weapon.damage.damageValue = damage.value;
            if (staminaCost != null) this.weapon.staminaCost = staminaCost.value;
            if (defenseRate != null) this.weapon.defenseRate = defenseRate.value;
            if (defenseRange != null) this.weapon.defenseRange = defenseRate.value;
        }
        public override void OnUnequip(vItem item)
        {
            if (!weapon) return;
            base.OnUnequip(item);
        }
    }
}
