using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BlocksSystem:MonoBehaviour
{
    [SerializeField] public Block[,,] Blocks;
    [HideInInspector] public int Size;
    [HideInInspector] public GameObject BlockPref;
    [HideInInspector] public float Delay;
    [HideInInspector] public Vector3 InitialVector;
    public bool Full=false;
    
    private void Start()
    {
        Size = EventManager.Instance.Size;
        BlockPref = EventManager.Instance.BlockPref;
        Delay = EventManager.Instance.SizeBox / Size;
        InitialVector = EventManager.Instance.InitialVector;
        EventManager.Instance.BlocksSystem = this;
        //
        Arrow.MoveBlock += MoveBlocks;
        //
        Blocks = new Block[Size, Size, Size];
        for (int x = 1; x < Size-1; x++)
        {
            for (int y = 1; y < Size-1; y++)
            {
                for (int z = 1; z < Size-1; z++)
                {
                    if (Full)
                    {
                        GameObject block = CreateBlock(x,y,z);
                        Blocks[x, y, z] = block.GetComponent<Block>();
                    }
                    else
                    {
                        Blocks[x, y, z] = null;
                    }
                }
            }
        }

        if (!Full)
        {
            CreateRandomBlock();
        }
    }

    private bool CheckFull()
    {
        for (int x = 1; x < Size-1; x++)
        {
            for (int y = 1; y < Size-1; y++)
            {
                for (int z = 1; z < Size-1; z++)
                {
                    if (Blocks[x, y, z] == null)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
    private GameObject CreateBlock(int x,int y,int z)
    {
        if (CheckFull())
        {
            GameObject block = Instantiate(BlockPref, transform);
            block.transform.localPosition = new Vector3(x * Delay, y * Delay, z * Delay);
            return block;
        }

        return null;
    }
    
    [ContextMenu("Создать блок рандомный")]
    private GameObject CreateRandomBlock()
    {
        if (CheckFull())
        {
            int x, y, z;
            do
            {
                x = (int)Random.Range(1, Size - 1);
                y = (int)Random.Range(1, Size - 1);
                z = (int)Random.Range(1, Size - 1);

            } while (Blocks[x, y, z] != null);

            GameObject block = CreateBlock(x, y, z);
            Blocks[x, y, z] = block.GetComponent<Block>();
            return block;
        }

        return null;
    }
    
    
    private void MoveBlocks(int x1,int y1,int z1)
    {
        bool x_axi=x1!=0;
        bool y_axi=y1!=0;
        bool z_axi=z1!=0;
        bool x_nega = x1 < 0;
        bool y_nega = y1 < 0;
        bool z_nega = z1 < 0;
        if(x_axi) MoveX(x1,y1,z1,x_nega);
        else if(y_axi) MoveY(x1,y1,z1,y_nega);
        else if(z_axi) MoveZ(x1,y1,z1,z_nega);
        CreateRandomBlock();
        CreateRandomBlock();
        CreateRandomBlock();
    }

    private bool Check(int x1, int x)
    {
        return (x + x1 < Size - 1 && x + x1 > 0);
    }
    
    private void MoveX(int x1, int y1, int z1, bool negative)
    {
        for (int x = 1; x < Size-1; x++)
        {
            for (int y = 1; y < Size-1; y++)
            {
                for (int z = 1; z < Size-1; z++)
                {
                    int _x = x;
                    int _y = y;
                    int _z = z;
                    if (negative) _x = Size - 1 - x;
                    
                    if (Blocks[_x,_y,_z]!=null&&(Check(x1,_x)&&Check(y1,_y)&&Check(z1,_z)))
                    {
                        if (Blocks[_x + x1, _y + y1, _z + z1] == null)
                        {
                            Blocks[_x + x1, _y + y1, _z + z1] = Blocks[_x, _y, _z];
                            Blocks[_x,_y,_z].BlockModule.MoveTo(_x + x1, _y + y1, _z + z1,Delay);
                            Blocks[_x, _y, _z] = null;
                        }
                        else if(Blocks[_x + x1, _y + y1, _z + z1].BlockModule.Num==Blocks[_x,_y,_z].BlockModule.Num)
                        {
                            Blocks[_x, _y, _z].BlockModule.MoveTo(_x + x1, _y + y1, _z + z1,Delay);
                            Blocks[_x + x1, _y + y1, _z + z1].BlockModule.NumIcr();
                            Destroy(Blocks[_x, _y, _z].gameObject);
                        }
                        else
                        {
                            
                        }
                    }
                }
            }
        }
    }
    private void MoveY(int x1, int y1, int z1, bool negative)
    {
        for (int x = 1; x < Size-1; x++)
        {
            for (int y = 1; y < Size-1; y++)
            {
                for (int z = 1; z < Size-1; z++)
                {
                    int _x = y;
                    int _y = x;
                    int _z = z;
                    if (negative) _y = Size - 1 - x;
                    
                    if (Blocks[_x,_y,_z]!=null&&(Check(x1,_x)&&Check(y1,_y)&&Check(z1,_z)))
                    {
                        if (Blocks[_x + x1, _y + y1, _z + z1] == null)
                        {
                            Blocks[_x + x1, _y + y1, _z + z1] = Blocks[_x, _y, _z];
                            Blocks[_x,_y,_z].BlockModule.MoveTo(_x + x1, _y + y1, _z + z1,Delay);
                            Blocks[_x, _y, _z] = null;
                        }
                        else if(Blocks[_x + x1, _y + y1, _z + z1].BlockModule.Num==Blocks[_x,_y,_z].BlockModule.Num)
                        {
                            Blocks[_x, _y, _z].BlockModule.MoveTo(_x + x1, _y + y1, _z + z1,Delay);
                            Blocks[_x + x1, _y + y1, _z + z1].BlockModule.NumIcr();
                            Destroy(Blocks[_x, _y, _z].gameObject);
                        }
                        else
                        {
                            
                        }
                    }
                }
            }
        }
    }
    private void MoveZ(int x1, int y1, int z1, bool negative)
    {
        for (int x = 1; x < Size-1; x++)
        {
            for (int y = 1; y < Size-1; y++)
            {
                for (int z = 1; z < Size-1; z++)
                {
                    int _x = z;
                    int _y = y;
                    int _z = x;
                    if (negative) _z = Size - 1 -x;
                    
                    if (Blocks[_x,_y,_z]!=null&&(Check(x1,_x)&&Check(y1,_y)&&Check(z1,_z)))
                    {
                        if (Blocks[_x + x1, _y + y1, _z + z1] == null)
                        {
                            Blocks[_x + x1, _y + y1, _z + z1] = Blocks[_x, _y, _z];
                            Blocks[_x,_y,_z].BlockModule.MoveTo(_x + x1, _y + y1, _z + z1,Delay);
                            Blocks[_x, _y, _z] = null;
                        }
                        else if(Blocks[_x + x1, _y + y1, _z + z1].BlockModule.Num==Blocks[_x,_y,_z].BlockModule.Num)
                        {
                            Blocks[_x, _y, _z].BlockModule.MoveTo(_x + x1, _y + y1, _z + z1,Delay);
                            Blocks[_x + x1, _y + y1, _z + z1].BlockModule.NumIcr();
                            Destroy(Blocks[_x, _y, _z].gameObject);
                        }
                        else
                        {
                            
                        }
                    }
                }
            }
        }
    }
}