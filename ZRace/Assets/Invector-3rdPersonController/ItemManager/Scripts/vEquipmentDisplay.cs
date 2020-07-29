using UnityEngine.UI;

namespace Invector.vItemManager
{   
    public class vEquipmentDisplay : vItemSlot
    {
        public Text slotIdentifier;
        public InputField.OnChangeEvent onChangeIdentifier;
        
        public void ItemIdentifier(int identifier = 0, bool showIdentifier = false)
        {
            if (!slotIdentifier) return;

            if(showIdentifier)
            {
                if(slotIdentifier)
                    slotIdentifier.text = identifier.ToString();
                onChangeIdentifier.Invoke(identifier.ToString());
            }
            else
            {
                if (slotIdentifier)
                    slotIdentifier.text = string.Empty;
                onChangeIdentifier.Invoke(string.Empty);
            }
        }
    }
}

