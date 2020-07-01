using Invector.vShooter;
using UnityEngine;

namespace Invector.vItemManager
{
    [vClassHeader("Shooter Equipment", openClose = false, useHelpBox = true, helpBoxText = "Use this component if you also use the ItemManager in your Character")]
    public class vShooterEquipment : vEquipment
    {
        vShooterWeapon _shooter;
        vMelee.vMeleeWeapon _melee;
        bool withoutShooterWeapon;
        bool withoutMeleeWeapon;

      
        protected virtual vShooterWeapon shooterWeapon
        {
            get
            {
                if (!_shooter && !withoutShooterWeapon)
                {
                    _shooter = GetComponent<vShooterWeapon>();
                    if (!_shooter) withoutShooterWeapon = true;
                }

                return _shooter;
            }
        }
        protected virtual vMelee.vMeleeWeapon meleeWeapon
        {
            get
            {
                if (!_melee && !withoutMeleeWeapon)
                {
                    _melee = GetComponent<vMelee.vMeleeWeapon>();
                    if (!_melee) withoutMeleeWeapon = true;
                }

                return _melee;
            }
        }
        public override void OnEquip(vItem item)
        {
            if (!shooterWeapon) return;
            base.OnEquip(item);
            shooterWeapon.changeAmmoHandle = new vShooterWeapon.ChangeAmmoHandle(ChangeAmmo);
            shooterWeapon.checkAmmoHandle = new vShooterWeapon.CheckAmmoHandle(CheckAmmo);
            var damageAttribute = item.GetItemAttribute(shooterWeapon.isSecundaryWeapon ? vItemAttributes.SecundaryDamage : vItemAttributes.Damage);

            if (damageAttribute != null)
            {
                shooterWeapon.maxDamage = damageAttribute.value;
            }

            if (shooterWeapon.secundaryWeapon)
            {
                var _equipments = shooterWeapon.secundaryWeapon.GetComponents<vEquipment>();
                for (int i = 0; i < _equipments.Length; i++)
                {
                    if (_equipments[i] != null) _equipments[i].OnEquip(item);
                }
            }
        }

        public override void OnUnequip(vItem item)
        {
            if (!shooterWeapon) return;
            base.OnUnequip(item);
            if (!item) return;           
            shooterWeapon.changeAmmoHandle = null;
            shooterWeapon.checkAmmoHandle = null;

            if (shooterWeapon.secundaryWeapon)
            {
                var _equipments = shooterWeapon.secundaryWeapon.GetComponents<vEquipment>();
                for (int i = 0; i < _equipments.Length; i++)
                {
                    if (_equipments[i] != null) _equipments[i].OnUnequip(item);
                }
            }
        }

        protected virtual bool CheckAmmo(ref bool isValid, ref int totalAmmo)
        {
            if (!referenceItem) return false;
            var damageAttribute = referenceItem.GetItemAttribute(shooterWeapon.isSecundaryWeapon ? vItemAttributes.SecundaryAmmoCount : vItemAttributes.AmmoCount);
            isValid = damageAttribute != null && !damageAttribute.isBool;
            if (isValid) totalAmmo = damageAttribute.value;
            return isValid && damageAttribute.value > 0;
        }

        protected virtual void ChangeAmmo(int value)
        {
            if (!referenceItem) return;
            var damageAttribute = referenceItem.GetItemAttribute(shooterWeapon.isSecundaryWeapon ? vItemAttributes.SecundaryAmmoCount : vItemAttributes.AmmoCount);

            if (damageAttribute != null)
            {
                damageAttribute.value += value;
            }
        }

    }
}