using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BlockPooler:MonoBehaviour,ISystem
{
    private int Size;
    private GameObject BlockPref;
    private GameObject[,,] BlockPoolerList;

    private void Start()
    {
        EventManager.Instance.Systems.Add(this);
        Size = EventManager.Instance.Size;
        BlockPref = EventManager.Instance.BlockPref;
        BlockPoolerList = new GameObject[Size, Size, Size];
        //
        for (int i = 0; i < Size-1; i++)
        {
            for (int j = 0; j < Size-1; j++)
            {
                for (int k = 0; k < Size-1; k++)
                {
                    // Debug.Log(transform);
                    GameObject obj = Instantiate(BlockPref,transform);
                    obj.SetActive(false);
                    BlockPoolerList[i, j, k]=obj;
                }
            }
        }
    }

    public GameObject GetBlock()
    {
        for (int i = 0; i < Size-1; i++)
        {
            for (int j = 0; j < Size-1; j++)
            {
                for (int k = 0; k < Size - 1; k++)
                {
                    // Debug.Log(BlockPoolerList[i,j,k].GetComponent<BlockModule>().Num);
                    if (BlockPoolerList[i,j,k]!=null&&!BlockPoolerList[i, j, k].activeInHierarchy)
                    {
                            return BlockPoolerList[i, j, k];
                    }
                }
            }
        }
        return null;
    }
    
    private void Restart()
    {
        for (int i = 0; i < Size-1; i++)
        {
            for (int j = 0; j < Size-1; j++)
            {
                for (int k = 0; k < Size-1; k++)
                {
                    BlockPoolerList[i,j,k].gameObject.SetActive(false);
                }
            }
        }
        Size = EventManager.Instance.Size;
        BlockPref = EventManager.Instance.BlockPref;
        BlockPoolerList = new GameObject[Size, Size, Size];
        //
        for (int i = 0; i < Size-1; i++)
        {
            for (int j = 0; j < Size-1; j++)
            {
                for (int k = 0; k < Size-1; k++)
                {
                    // Debug.Log(transform);
                    GameObject obj = Instantiate(BlockPref,transform);
                    obj.SetActive(false);
                    BlockPoolerList[i, j, k]=obj;
                }
            }
        }
    }

    public void GameOver()
    {
        return;
    }

    void ISystem.Continue()
    {
        Restart();
    }
    
    void ISystem.Restart()
    {
        Restart();
    }
}