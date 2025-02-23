using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
public class CameraOthoPerspLerp : MonoBehaviour
{
    public static Matrix4x4 MatrixLerp(Matrix4x4 _start, Matrix4x4 _end, float _t)
    {
        Matrix4x4 _res = new Matrix4x4();
        for (int i = 0; i < 16; i++) _res[i] = Mathf.Lerp(_start[i], _end[i], _t);
        return _res;
    }
    private IEnumerator LerpFromTo(Matrix4x4 _start, Matrix4x4 _dest, float _deltaT)
    {
        float _startTime = Time.time;
        while (Time.time - _startTime < _deltaT)
        {
            Camera.main.projectionMatrix = MatrixLerp(_start, _dest, (Time.time - _startTime) / _deltaT);
            yield return 1;
        }
        Camera.main.projectionMatrix = _dest;
    }

    public Coroutine BlendToMatrix(Matrix4x4 _targetMatrix, float _deltaT)
    {
        StopAllCoroutines();
        return StartCoroutine(LerpFromTo(Camera.main.projectionMatrix, _targetMatrix, _deltaT));
    }
    
}
