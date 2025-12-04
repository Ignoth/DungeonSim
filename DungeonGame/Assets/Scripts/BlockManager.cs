using System;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    Vector2 fieldRangeLowerLeft;
    [SerializeField]
    Vector2 fieldRangeUpperRight;
    int fieldWidth = 1;
    int fieldHeight = 1;
    GameObject[,] blocks;
    [SerializeField]
    GameObject[] blocksVariations;

    void Start()
    {
        fieldWidth = Mathf.Abs((int)Mathf.Round(fieldRangeLowerLeft.x - fieldRangeUpperRight.x));
        fieldHeight = Mathf.Abs((int)Mathf.Round(fieldRangeLowerLeft.y - fieldRangeUpperRight.y));
        Debug.Log("Field Width: " + fieldWidth + ", Field Height: " + fieldHeight);
        blocks = new GameObject[fieldWidth, fieldHeight];
        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                blocks[x, y] = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
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
