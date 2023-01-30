using UnityEngine;

public class ViewPanel : MonoBehaviour
{
    public void EnableView() => gameObject.SetActive(true);

    public void DisableView() => gameObject.SetActive(false);
}
