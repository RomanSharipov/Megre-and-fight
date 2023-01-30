using System.Collections;
using IJunior.TypedScenes;
using UnityEngine;

public class InitialLoad : MonoBehaviour
{
    private const float ApproximateLoadTime = 2f;

    private void Start()
    {
        StartCoroutine(WaitForLoad());
    }

    private IEnumerator WaitForLoad()
    {
        yield return new WaitForSeconds(ApproximateLoadTime);

        MergeScene.Load();
    }
}
