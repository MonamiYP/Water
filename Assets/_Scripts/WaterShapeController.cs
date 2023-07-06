using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class WaterShapeController : MonoBehaviour {

    private int cornersCount = 2;
    [SerializeField][Range(1, 100)] private int wavesCount = 6;
    [SerializeField] SpriteShapeController spriteShapeController;

    [SerializeField] private GameObject wavePointPrefab;
    [SerializeField] private GameObject wavePoints;

    [SerializeField] private List<Spring> springs = new();
    [SerializeField] private float springStiffness = 0.1f;
    [SerializeField] private float dampening = 0.03f;
    [SerializeField] private float spread = 0.006f;

    void OnValidate() {
        StartCoroutine(CreateWaves());
    }
    IEnumerator CreateWaves() {
        foreach (Transform child in wavePoints.transform) {
            StartCoroutine(Destroy(child.gameObject));
        }
        yield return null;
        SetWaves();
        yield return null;
    }
    IEnumerator Destroy(GameObject _gameObject) {
        yield return null;
        DestroyImmediate(_gameObject);
    }

    private void SetWaves() {
        Spline waterSpline = spriteShapeController.spline;
        int waterPointsCount = waterSpline.GetPointCount();

        for (int i = cornersCount; i < waterPointsCount - cornersCount; i++) {
            waterSpline.RemovePointAt(cornersCount);
        }

        Vector3 waterTopLeftCorner = waterSpline.GetPosition(1);
        Vector3 waterTopRightCorner = waterSpline.GetPosition(2);
        float waterWidth = waterTopRightCorner.x - waterTopLeftCorner.x;

        float spacingPerWave = waterWidth / (wavesCount + 1);

        for (int i = wavesCount; i > 0; i--) {
            int index = cornersCount;

            float xPos = waterTopLeftCorner.x + (spacingPerWave * i);
            Vector3 wavePoint = new Vector3(xPos, waterTopLeftCorner.y, waterTopLeftCorner.z);
            waterSpline.InsertPointAt(index, wavePoint);
            waterSpline.SetHeight(index, 0.1f);
            waterSpline.SetCorner(index, false);
            waterSpline.SetTangentMode(index, ShapeTangentMode.Continuous);
        }

        CreateSprings(waterSpline);
    }

    private void CreateSprings(Spline waterSpline) {
        springs = new();
        for (int i = 0; i <= wavesCount + 1; i++) {
            SmoothenTangents(waterSpline, i + 1);
            GameObject wavePoint = Instantiate(wavePointPrefab, wavePoints.transform, false);
            wavePoint.transform.localPosition = waterSpline.GetPosition(i + 1);
            Spring waterSpring = wavePoint.GetComponent<Spring>();
            waterSpring.Init(spriteShapeController);
            springs.Add(waterSpring);
        }
    }

    private void FixedUpdate() {
        foreach (Spring waterSpring in springs) {
            waterSpring.SpringUpdate(springStiffness, dampening);
            waterSpring.SineWave();
            waterSpring.PositionUpdate();
            waterSpring.SplineUpdate();
        }
        UpdateSpringNeighbours();
    }

    private void UpdateSpringNeighbours() {
        for (int i = 0; i < springs.Count; i ++) {
            if (i > 0) {
                springs[i - 1].velocity += (springs[i].height - springs[i - 1].height) * spread;
            }
            if (i < springs.Count - 1) {
                springs[i + 1].velocity += (springs[i].height - springs[i + 1].height) * spread;
            }
        }
    }

    private void SmoothenTangents(Spline waterSpline, int index)
    {
        Vector3 position = waterSpline.GetPosition(index);
        Vector3 positionPrev = position;
        Vector3 positionNext = position;
        if (index > 1) {
            positionPrev = waterSpline.GetPosition(index-1);
        }
        if (index - 1 <= wavesCount) {
            positionNext = waterSpline.GetPosition(index+1);
        }

        Vector3 forward = gameObject.transform.forward;

        float scale = Mathf.Min((positionNext - position).magnitude, (positionPrev - position).magnitude) * 0.33f;

        Vector3 leftTangent = (positionPrev - position).normalized * scale;
        Vector3 rightTangent = (positionNext - position).normalized * scale;

        SplineUtility.CalculateTangents(position, positionPrev, positionNext, forward, scale, out rightTangent, out leftTangent);
        
        waterSpline.SetLeftTangent(index, leftTangent);
        waterSpline.SetRightTangent(index, rightTangent);
    }
}
