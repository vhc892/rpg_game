using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string entranceID;

    private void Start()
    {
        string targetID = LevelManager.Instance.GetEntranceID();

        if (entranceID == targetID)
        {
            PlayerController.Instance.transform.position = transform.position;
            CameraController.Instance.SetPlayerCameraFollow();
            UIFade.Instance.FadeToClear();
        }
    }
}
