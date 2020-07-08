using Invector;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using Invector.vItemManager;
using System.Collections.Generic;
using UnityEngine;

[vClassHeader("Build Manager", "This component requires a ShooterController, ItemManager and a GenericAction. Make sure to add this as a child of the controller.", openClose = false)]
public class vBuildManager : vMonoBehaviour
{
    [vReadOnly]
    public List<vBuildObject> builds;
    [vReadOnly]
    public vBuildObject currentBuild;
    public vItemManager itemManager;
    public vGenericAction genericAction;
    
    [vHelpBox("Optional - You can spawn a item instantly or use the TriggerGenericAction to play a animation first and spawn after")]
    public GenericInput createBuildInput = new GenericInput("E", "X", "X");
    [Space]
    public GenericInput enterBuildMode = new GenericInput("B", "A", "A");
    public GenericInput rotateLeft = new GenericInput("Alpha1", "LB", "LB");
    public GenericInput rotateRight = new GenericInput("Alpha2", "RB", "RB");
    public bool rotateToCameraFwdWhenBuild = true;
    public UnityEngine.Events.UnityEvent onIsItemEquipped, onIsItemUnequipped, onEnterBuildMode, onExitBuildMode;

    public bool inBuildMode;
    
    protected vThirdPersonInput tpInput;
    
    void Start()
    {
        tpInput = GetComponentInParent<vThirdPersonInput>();
        if (tpInput != null)
        {
            tpInput.onUpdate -= UpdateBuilderBehavior;
            tpInput.onUpdate += UpdateBuilderBehavior;
        }
        itemManager = GetComponentInParent<vItemManager>();
        genericAction = GetComponentInParent<vGenericAction>();

        var _builds = GetComponentsInChildren<vBuildObject>();
        builds.Clear();
        for (int i = 0; i < _builds.Length; i++)
        {
            var item = itemManager.itemListData.items.Find(_item => _item.id.Equals(_builds[i].id));
            if (item != null)
            {
                builds.Add(_builds[i]);
                _builds[i].onSpawnBuild.AddListener(OnSpawnBuildObject);
                _builds[i].spawnPrefab = item.originalObject;
            }
        }
    }

    void UpdateBuilderBehavior()
    {
        CheckBuilds();
        CheckInput();
        HandleBuild();
        LockInventoryInput();
        ItemEquippedEvents();
    }

    public virtual void LockInventoryInput()
    {
        if (itemManager)
            itemManager.inventory.lockInventoryInput = inBuildMode;
    }

    public virtual void ItemEquippedEvents()
    {
        if (!currentBuild) return;

        if (currentBuild.canCreate)
            onIsItemEquipped.Invoke();
        else
            onIsItemUnequipped.Invoke();
    }

    public virtual void HandleBuildRotation()
    {
        if (rotateLeft.GetButton())
            currentBuild.angle -= Time.deltaTime * currentBuild.rotateSpeed;
        else if (rotateRight.GetButton())
            currentBuild.angle += Time.deltaTime * currentBuild.rotateSpeed;
    }

    public virtual void HandleBuild()
    {
        if (!inBuildMode || !currentBuild)
            return;
        if (!string.IsNullOrEmpty(currentBuild.customCameraState))
            tpInput.ChangeCameraState(currentBuild.customCameraState, true);

        tpInput.MoveInput();       

        currentBuild.HandleBuild(this);
        HandleBuildRotation();
    }

    public virtual void CheckBuilds()
    {
        for (int i = 0; i < builds.Count; i++)
        {
            builds[i].canCreate = itemManager.ItemIsEquiped(builds[i].id);
            if (builds[i].canCreate)
            {
                currentBuild = builds[i];
                break;
            }
        }
    }

    public virtual void CheckInput()
    {
        if (currentBuild && currentBuild.canCreate)
        {
            if (enterBuildMode.GetButtonDown())
            {
                inBuildMode = !inBuildMode;

                if (inBuildMode)
                {
                    EnterBuildMode();
                }
                else
                {
                    ExitBuildMode();
                }
            }
        }
        else if (inBuildMode)
        {
            inBuildMode = false;
            if (currentBuild)
                ExitBuildMode();
        }
    }

    public virtual void EnterBuildMode()
    {       
        tpInput.SetLockAllInput(true);
        currentBuild.EnterBuild();
        if (currentBuild.strafeWhileCreate && tpInput.cc.locomotionType != vThirdPersonMotor.LocomotionType.OnlyStrafe)
            tpInput.SetStrafeLocomotion(true);
        onEnterBuildMode.Invoke();
        genericAction.SetLockTriggerEvents(true);
    }

    public virtual void ExitBuildMode()
    {        
        tpInput.SetLockAllInput(false);
        currentBuild.ExitBuild();
        if (currentBuild.strafeWhileCreate && tpInput.cc.locomotionType != vThirdPersonMotor.LocomotionType.OnlyStrafe)
            tpInput.SetStrafeLocomotion(false);
        tpInput.ResetCameraState();
        onExitBuildMode.Invoke();
        genericAction.SetLockTriggerEvents(false);
    }

    public virtual void OnSpawnBuildObject()
    {        
        tpInput.SetLockAllInput(false);
        if (currentBuild.strafeWhileCreate && tpInput.cc.locomotionType != vThirdPersonMotor.LocomotionType.OnlyStrafe)
            tpInput.SetStrafeLocomotion(false);
        tpInput.ResetCameraState();
        onExitBuildMode.Invoke();
        genericAction.SetLockTriggerEvents(false);
        if (!currentBuild) return;
        inBuildMode = false;
        vItemListOperations.EquipedItemInfo equipedItemInfo = null;
        if (itemManager.ItemIsEquipped(currentBuild.id, out equipedItemInfo))
        {
            itemManager.DestroyItem(equipedItemInfo.item, 1);
        }
    }
}