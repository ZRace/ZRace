using Invector;
using Invector.vCharacterController.vActions;
using Invector.vItemManager;
using System.Collections.Generic;
using UnityEngine;

[vClassHeader("Build Object", "Make sure to replace the ItemID to actually get the correct SpawnPrefab, you can assign the SpawnPrefab on your ItemListData", openClose = false)]
public class vBuildObject : vMonoBehaviour
{
    #region Variables

    [Tooltip("Layers to create on")]
    public LayerMask layer = 1 << 0;
    [Tooltip("Tags that will be ignored")]
    public List<string> tagsToExclude = new List<string>() { "Player", "Action", "IgnoreRagdoll", "Weapon" };

    public Color validColor = new Color(0, 1, 0, 0.3f);
    public Color invalidColor = new Color(1, 0, 0, 0.3f);

    public Renderer _renderer;
    public BoxCollider _collider;
    public bool alwaysShowGizmos;
    [Header("Optional")]
    public bool strafeWhileCreate;
    public Transform target;
    public string customCameraState;
    public float rotateSpeed = 3f;
    public UnityEngine.Events.UnityEvent onEnterBuilding, onExitBuilding, onSpawnBuild;
    public OnValidateTrapCreation onValidateTrap;
    [System.Serializable]
    public class OnValidateTrapCreation : UnityEngine.Events.UnityEvent<bool> { }

    [vHelpBox("Use a TriggerGenerecAction to play a custom animation or leave it unassign to spawn the prefab instantly.")]
    public vTriggerGenericAction trigger;
    public GameObject spawnPrefab;
    Transform parent;
    public float angle = 0;

    Vector3 lastValidPos;
    bool inBuildMode;
    bool isValid;
    Bounds myBounds;

    [vHelpBox("Make sure your Item is assign with a SpawnObject Prefab in the ItemListData")]
    public int id;
    public float creationDistance = 1;
    public float animationDistance;
    public Vector3 boundSize = Vector3.one;
    public float offsetY;
    public float offsetAngle;
    public float offsetAnglePreview;
    [Range(0.1f, 1f)]
    public float rayRadius = 0.2f;
    public float rayHeight = 0.5f;
    [HideInInspector]
    public bool canSpawn, canCreate, canUpdateBuild;

    public vItemListOperations.EquipedItemInfo equipedItemInfo;
    public vBuilderCollisionSensor collisionSensor;
    public float collisionSensorHeight = 0.5f;
    public float collisionSensorRadius = 0.25f;

    public BoxPoints boxPoints = new BoxPoints();

    public bool debugMode;

    #endregion

    private void OnEnable()
    {
        canSpawn = true;
    }

    private void Awake()
    {
        parent = transform.parent;
        if (trigger)
        {
            ///Add event to <seealso cref="vTriggerGenericAction"/>
            trigger.OnPressActionInput.AddListener(() => { SetActiveUpdateTrap(false); });
            trigger.OnCancelActionInput.AddListener(() => { SetActiveUpdateTrap(true); });
            if (trigger.inputType == vTriggerGenericAction.InputType.GetButtonTimer)
                trigger.OnFinishActionInput.AddListener(() => { CreateBuild(); SetActiveUpdateTrap(true); });
            else trigger.OnEndAnimation.AddListener(() => { CreateBuild(); SetActiveUpdateTrap(true); });
            trigger.gameObject.SetActive(false);

            //_collider = GetComponentInParent<BoxCollider>();
        }
    }

    public struct BoxPoints
    {
        public Vector3 pR_F, pL_F, pR_B, pL_B;
        public Vector3 center, right, forward;
        RaycastHit hR_F, hR_B, hL_F, hL_B, hCenter;
        public void RayCastPoints(out Vector3 position, out Vector3 normal, LayerMask mask, float radius = 0.1f, float height = 2f)
        {
            position = center;
            Vector3 p0 = center + Vector3.up * height;
            Vector3 p1 = pR_F + Vector3.up * height;
            Vector3 p2 = pR_B + Vector3.up * height;
            Vector3 p3 = pL_F + Vector3.up * height;
            Vector3 p4 = pL_B + Vector3.up * height;

            if (Physics.Linecast(center, p0, out hCenter, mask)) p0 = hCenter.point;
            if (Physics.Linecast(pR_F, p1, out hR_F, mask)) p1 = hR_F.point;
            if (Physics.Linecast(pR_B, p2, out hR_B, mask)) p2 = hR_B.point;
            if (Physics.Linecast(pL_F, p3, out hL_F, mask)) p3 = hL_F.point;
            if (Physics.Linecast(pL_B, p4, out hL_B, mask)) p4 = hL_B.point;

            if (Physics.Raycast(p0, Vector3.down, out hCenter, 4, mask)) position = hCenter.point;
            if (Physics.SphereCast(p1, radius, Vector3.down, out hR_F, 4, mask)) pR_F = hR_F.point;
            if (Physics.SphereCast(p2, radius, Vector3.down, out hR_B, 4, mask)) pR_B = hR_B.point;
            if (Physics.SphereCast(p3, radius, Vector3.down, out hL_F, 4, mask)) pL_F = hL_F.point;
            if (Physics.SphereCast(p4, radius, Vector3.down, out hL_B, 4, mask)) pL_B = hL_B.point;

            Vector3 meddlePoint = (pR_F + pR_B + pL_F + pL_B) / 4;
            if (meddlePoint.y > position.y) position = meddlePoint;
            Vector3 upDir = (-GetNormal(pL_F, pL_B, pR_F) +
                             -GetNormal(pR_F, pL_F, pR_B) +
                             -GetNormal(pR_B, pR_F, pL_B) +
                             -GetNormal(pL_B, pR_B, pL_F)).normalized;
            normal = upDir;

        }
        Vector3 GetNormal(Vector3 a, Vector3 b, Vector3 c)
        {
            // Find vectors corresponding to two of the sides of the triangle.
            Vector3 side1 = b - a;
            Vector3 side2 = c - a;

            // Cross the vectors to get a perpendicular vector, then normalize it.
            return Vector3.Cross(side1, side2).normalized;
        }
        public void UpdateValues(Transform transform, Vector3 right, Vector3 forward, BoxCollider _collider, float radius = 0.1f)
        {
            center = transform.position;
            this.right = right * ((_collider.size.x * 0.5f) - radius);
            this.forward = forward * ((_collider.size.z * 0.5f) - radius);
            pR_F = center + this.right + this.forward;
            pL_F = center - this.right + this.forward;
            pR_B = center + this.right - this.forward;
            pL_B = center - this.right - this.forward;
        }
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (_collider && ((Application.isPlaying && inBuildMode) || (alwaysShowGizmos || UnityEditor.Selection.activeGameObject == gameObject)))
        {
            Gizmos.color = (canSpawn ? Color.green : Color.red) * 0.5f;

            if (!Application.isPlaying)
            {
                var forward = Quaternion.AngleAxis(angle + offsetAngle, transform.up) * _collider.transform.forward;
                Vector3 right = Quaternion.AngleAxis(90, transform.up) * forward;
                boxPoints.UpdateValues(transform, right, forward, _collider, rayRadius);
            }
            Gizmos.DrawSphere(boxPoints.pR_F, rayRadius);
            Gizmos.DrawSphere(boxPoints.pL_F, rayRadius);
            Gizmos.DrawSphere(boxPoints.pR_B, rayRadius);
            Gizmos.DrawSphere(boxPoints.pL_B, rayRadius);
            Gizmos.matrix = Matrix4x4.TRS(_collider.transform.position, _collider.transform.rotation, (_collider.transform.lossyScale));
            Gizmos.DrawCube(_collider.center, _collider.size);
        }
#endif
    }

    /// <summary>
    /// Enter Build Mode 
    /// </summary>
    public virtual void EnterBuild()
    {
        if (debugMode) Debug.Log("Enter Build");
        if (collisionSensor)
        {
            collisionSensor.onTriggerStay.AddListener(OnTriggerStay);
            collisionSensor.onTriggerExit.AddListener(OnTriggerExit);
        }
        if (trigger != null)
            trigger.gameObject.SetActive(true);
        onEnterBuilding.Invoke();                                  // call onEnterBuildMove Event
        canUpdateBuild = true;
        transform.parent = null;
        _collider.gameObject.SetActive(true);
        inBuildMode = true;
    }

    /// <summary>
    /// Exit Build Mode 
    /// </summary>
    public virtual void ExitBuild()
    {
        if (debugMode) Debug.Log("Exit Build");
        if (collisionSensor)
        {
            collisionSensor.onTriggerStay.RemoveListener(OnTriggerStay);
            collisionSensor.onTriggerExit.RemoveListener(OnTriggerExit);
        }
        onExitBuilding.Invoke();
        _collider.gameObject.SetActive(false);
        if (trigger != null)
            trigger.gameObject.SetActive(false);
        if (transform.parent == null) transform.parent = parent;
        inBuildMode = false;
    }

    /// <summary>
    /// Update the Build Object position, rotation and check if can spawn the BuildObject prefab
    /// </summary>
    /// <param name="bm"></param>
    public virtual void HandleBuild(vBuildManager bm)
    {
        if (isValid != canSpawn)
        {
            isValid = canSpawn;
            onValidateTrap.Invoke(canSpawn);
        }
        if (canUpdateBuild)
        {
            ///BuildObject preview color
            _renderer.sharedMaterial.color = isValid ? validColor : invalidColor;

            Debug.DrawRay(parent.position, Vector3.up, Color.red, 0.01f);
            ///Calc the position of the BuildObject
            var dir = parent.position - Camera.main.transform.position;
            dir.y = 0;
            var position = parent.position + dir.normalized * creationDistance;

            var distance = Vector3.Distance(position, _collider.ClosestPoint(parent.position));
            position = position + dir.normalized * distance;

            Debug.DrawRay(position, Vector3.up, Color.green, 0.1f);

            ///Update Collision Sensor position, rotation and height to prevent Walls and other obstacles
            if (collisionSensor)
            {
                collisionSensor.transform.position = position + Vector3.up * ((collisionSensor._capsuleCollider.radius * 2f) + collisionSensorHeight);
                var dist = Vector3.Distance(position, parent.position);
                collisionSensor._capsuleCollider.height = dist;
                collisionSensor._capsuleCollider.radius = collisionSensorRadius;
                collisionSensor._capsuleCollider.center = new Vector3(0f, 0f, -(dist * 0.5f));
            }

            ///Check Surface to plant BuildObject to recalc position if necessary
            RaycastHit hit;
            if (Physics.Raycast(position + transform.up * boundSize.y, Vector3.down, out hit, boundSize.y + 2f, layer))
            {
                position.y = hit.point.y;
            }

            Debug.DrawRay(position, Vector3.up, Color.blue, 0.1f);

            ///Update Position and rotation of the BuildObject
            if (canSpawn) lastValidPos = position;
            transform.position = position;
            transform.localScale = boundSize;

            AlignToSurface(dir.normalized);
            ///Update Match Target of the <seealso cref="vTriggerGenericAction"/> to correct position and rotation
            if (target && trigger)
            {
                target.position = position - dir.normalized * (animationDistance + distance);
                target.LookAt(transform.position, Vector3.up);
                Debug.DrawRay(target.position, target.up);
                Debug.DrawRay(target.position, target.right);
                Debug.DrawRay(target.position, -target.right);
                Debug.DrawRay(target.position, target.forward);
                Debug.DrawRay(target.position, -target.forward);
            }
            //Rotate the Preview  of the BuildObject
            //_renderer.transform.localEulerAngles = new Vector3(0, angle, 0);
        }

        /// Instantly instantiate the prebab without an trigger  <seealso cref="vGenericAction"/> 
        if (!trigger && bm.createBuildInput.GetButtonDown() && isValid)
        {
            CreateBuild();
        }
        else if (trigger && (isValid || bm.genericAction.playingAnimation))
        {
            ///Fix Position of build object to last valid position befoure animation started 
            if (bm.genericAction.playingAnimation)
                transform.position = lastValidPos;
            ///Simulate <seealso cref="vGenericAction"/>  Input Behaviour
            bm.genericAction.triggerAction = trigger;
            bm.genericAction.TriggerActionInput();
        }
    }

    /// <summary>
    /// Align the collider to the surface
    /// </summary>
    public virtual void AlignToSurface(Vector3 forward)
    {
        if (!_collider) return;

        forward = Quaternion.AngleAxis(angle + offsetAngle, transform.up) * forward;
        Vector3 right = Quaternion.AngleAxis(90, transform.up) * forward;
        boxPoints.UpdateValues(transform, right, forward, _collider, rayRadius);

        Vector3 position;
        Vector3 normal;
        boxPoints.RayCastPoints(out position, out normal, layer, rayRadius, rayHeight);

        var myForward = Vector3.Cross(right, normal);
        var lookFwd = Quaternion.AngleAxis(offsetAnglePreview, normal) * myForward;
        if (lookFwd != Vector3.zero)
        {
            var rotation = Quaternion.LookRotation(lookFwd, normal);
            _collider.transform.rotation = Quaternion.Lerp(_collider.transform.rotation, rotation, 20 * Time.deltaTime);
        }

        Vector3 p = position + normal.normalized * offsetY;
        _collider.transform.position = Vector3.Lerp(_collider.transform.position, p, 20 * Time.deltaTime);
    }

    /// <summary>
    /// Instantiate the Prefab of the buildObject
    /// </summary>
    public void CreateBuild()
    {
        Instantiate(spawnPrefab, _collider.transform.position, _collider.transform.rotation);
        ExitBuild();
        onSpawnBuild.Invoke();
    }

    /// <summary>
    /// Enable or disable Update 
    /// </summary>
    /// <param name="value"></param>
    public void SetActiveUpdateTrap(bool value)
    {
        canUpdateBuild = value;
        if (!value)
            canSpawn = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (canUpdateBuild && !tagsToExclude.Contains(other.gameObject.tag))
        {
            if (_renderer.enabled)
            {
                canSpawn = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!tagsToExclude.Contains(other.gameObject.tag))
        {
            if (_renderer.enabled)
            {
                canSpawn = true;
            }
        }
    }
}