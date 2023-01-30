using System.Collections;
using UnityEngine;

public class LoadingScreenAnimation : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _delayBetweenTicks = 0.5f;
    [SerializeField] private string _prefix = "Loading";
    [SerializeField] private string _repeatableSuffix = ".";
    [SerializeField] private TMPro.TMP_Text _text;

    private void OnEnable()
    {
        StartCoroutine(TextAnimation());
    }

    private IEnumerator TextAnimation()
    {
        var tick = new WaitForSeconds(_delayBetweenTicks);

        while (enabled)
        {
            _text.text = _prefix + _repeatableSuffix;
            yield return tick; 
            
            _text.text = _prefix + _repeatableSuffix + _repeatableSuffix;
            yield return tick;

            _text.text = _prefix + _repeatableSuffix + _repeatableSuffix + _repeatableSuffix;
            yield return tick;
        }
    }
}
