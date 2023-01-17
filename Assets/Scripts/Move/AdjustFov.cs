using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class AdjustFov : MonoBehaviour
{
    public PostProcessVolume FoV;
    private Vignette _vignette;
    private IEnumerator _coroutine;
    private bool _isChanged = false;
    private float fovX = 0.5f;
    private float fovY = 0.5f;

    private void Awake()
    {
        FoV.profile.TryGetSettings(out _vignette);
        _isChanged = false;
    }
    private void Update()
    {
        if(_isChanged)
            UpdateFov();
    }
    public void UpdateFovX(float x)
    {
        if (fovX == x)
            return;
        _isChanged = true;
        fovX = x;
    }
    public void UpdateFovY(float y)
    {
        if (fovY == y)
            return;
        _isChanged = true;
        fovY = y;
    }
    private void UpdateFov()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = FovAdjustion(fovX, fovY);
        StartCoroutine(_coroutine);
        _isChanged = false;
    }
    private IEnumerator FovAdjustion(float x, float y)
    {
        if (!FoV.gameObject.activeInHierarchy)
            FoV.gameObject.SetActive(true);
        float initx = _vignette.center.value.x;
        float inity = _vignette.center.value.y;
        for (float f = 0f; f <= 0.25f; f += Time.deltaTime)
        {
            _vignette.center.value.x = Mathf.Lerp(initx, x, f / 0.25f);
            _vignette.center.value.y = Mathf.Lerp(inity, y, f / 0.25f);
            yield return null;
        }
        _vignette.center.value.x = x;
        _vignette.center.value.y = y;
        if (_vignette.center.value.x == 0.5f && _vignette.center.value.y == 0.5f)
            FoV.gameObject.SetActive(false);
    }
}
