using UnityEngine;

namespace CBGames.UI
{
    [AddComponentMenu("CB GAMES/Camera/Look At Camera")]
    public class LookAtCamera : MonoBehaviour
    {
        /// <summary>
        /// Just always look at the main camera.
        /// </summary>
        private void Update()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
