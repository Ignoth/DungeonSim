using UnityEngine;
using UnityEngine.UI;

public class HeatManager : MonoBehaviour
{
    public int W;               // グリッドの幅
    public int H;               // グリッドの縦幅
    public int N;               // セルの数
    public float D;             // 熱拡散率
    public float dt;            // 時間ステップ
    public float[,] heatGrid;       // 温度配列（現在）
    public float[,] nextHeatGrid;       // 温度配列(次のステップ)
    public float dx;            // セルの大きさ

    [SerializeField] GameObject heatMapPrefab;
    public GameObject[,] heatMapObjects;

    void Start()
    {
        heatGrid = new float[W, H];
        nextHeatGrid = new float[W, H];
        heatMapObjects = new GameObject[W, H];

        for (int x = 0; x < W; x++)
        {
            for (int y = 0; y < H; y++)
            {
                heatMapObjects[x, y] = Instantiate(heatMapPrefab, new Vector3(x, 0f, y), heatMapPrefab.transform.rotation);
            }
        }
        HeatMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            int x = Random.Range(0, W);
            int y = Random.Range(0, H);
            heatGrid[x, y] += 1f;
            Debug.Log($"{x},{y}を{heatGrid[x, y]}度に加熱");
        }

        StepHeat();     // 次のステップのシミュレーション
        HeatMap();      // ヒートマップの表示を更新
    }

    void StepHeat()
    {
        float alpha = D * dt / (dx * dx);

        for (int x = 1; x < W - 1; x++)
        {
            for (int y = 1; y < H - 1; y++)
            {
                float t = heatGrid[x, y];

                float left = (x > 0) ? heatGrid[x - 1, y] : t;
                float right = (x < W - 1) ? heatGrid[x + 1, y] : t;
                float down = (y > 0) ? heatGrid[x, y - 1] : t;
                float up = (y < H - 1) ? heatGrid[x, y + 1] : t;

                float lap = left + right + up + down - 4 * t;

                nextHeatGrid[x, y] = t + alpha * lap;

                /*
                next[x, y] =
                    grid[x, y] +
                    alpha * (
                        grid[x + 1, y] +
                        grid[x - 1, y] +
                        grid[x, y + 1] +
                        grid[x, y - 1]
                        - 4 * grid[x, y]
                    );
                */
            }
        }

        // swap
        var tmp = heatGrid;
        heatGrid = nextHeatGrid;
        nextHeatGrid = tmp;
    }

    void HeatMap()
    {
        for (int x = 0; x < W; x++)
        {
            for (int y = 0; y < H; y++)
            {
                MeshRenderer meshRenderer = heatMapObjects[x, y].GetComponent<MeshRenderer>();
                heatMapObjects[x, y].GetComponentInChildren<TextMesh>().text = (Mathf.Floor(heatGrid[x, y] * 1000f) / 1000f).ToString();
                meshRenderer.material.color = new Color(TemperatureToColor(heatGrid[x, y]).r, TemperatureToColor(heatGrid[x, y]).g, TemperatureToColor(heatGrid[x, y]).b, meshRenderer.material.color.a);
                heatMapObjects[x, y].GetComponentInChildren<TextMesh>().color = TemperatureToColor(heatGrid[x, y]);
            }
        }
    }

    Color TemperatureToColor(float t)
    {
        // t を 0から1 の範囲に正規化（好みで調整）
        t = Mathf.Clamp01(t);

        // 青 > 赤 > 白 のグラデーション
        if (t < 0.5f)
        {
            // 0 > 0.5 : 青 > 赤
            float k = t / 0.5f;
            return new Color(k, 0, 1f - k);
        }
        else
        {
            // 0.5 > 1.0 : 赤 > 白
            float k = (t - 0.5f) / 0.5f;
            return new Color(1f, k, k);
        }
    }

}
