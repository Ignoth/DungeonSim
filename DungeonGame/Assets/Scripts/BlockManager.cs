using System;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] Vector2 fieldRangeLowerLeft;
    [SerializeField] Vector2 fieldRangeUpperRight;
    public int fieldWidth;
    public int fieldHeight;
    public GameObject[,] blocks;
    [SerializeField]
    GameObject[] blocksVariations;

    HeatManager heatManager;

    void Awake()
    {
        heatManager = GetComponent<HeatManager>();

        fieldWidth = Mathf.Abs((int)Mathf.Round(fieldRangeLowerLeft.x - fieldRangeUpperRight.x));
        fieldHeight = Mathf.Abs((int)Mathf.Round(fieldRangeLowerLeft.y - fieldRangeUpperRight.y));
        heatManager.W = fieldWidth;
        heatManager.H = fieldHeight;
        heatManager.N = heatManager.W * heatManager.H;
    }

    private void Start()
    {
        Debug.Log("Field Width: " + fieldWidth + ", Field Height: " + fieldHeight);
        blocks = new GameObject[fieldWidth, fieldHeight];
        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                blocks[x, y] = null;
            }
        }

        for (int i = 0; i < 20; i++)
        {
            int x = UnityEngine.Random.Range(0, fieldWidth);
            int y = UnityEngine.Random.Range(0, fieldHeight);
            int index = UnityEngine.Random.Range(0, blocksVariations.Length);
            if (blocks[x, y] == null)
            {
                blocks[x, y] = Instantiate(blocksVariations[index], new Vector3(x, 0f, y), Quaternion.identity);
            }
            else
            {
                i--;
                continue;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // blocks‚Ì’†g‚ðDebug.Log‚Å•\Ž¦
        if (Input.GetKeyDown(KeyCode.K))
        {
            string blockArrayGrid = "";
            for (int x = 0; x < fieldWidth; x++)
            {
                for (int y = 0; y < fieldHeight; y++)
                {
                    if (blocks[x, y] != null)
                    {
                        blockArrayGrid += Array.IndexOf(blocksVariations, blocks[x,y]);
                    }
                    else
                    {
                        blockArrayGrid += "- ";
                    }
                }
                blockArrayGrid += "\n";
            }
            Debug.Log(blockArrayGrid);
        }
    }
}
