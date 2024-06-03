using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BlocksSystem:MonoBehaviour,ISystem
{
    [HideInInspector] public Block[,,] Blocks;
    public int Size;
    public GameObject BlockEmptyPref;
    public float Delay;
    public int SpawnRate;
    public int StopRateBase;
    public Action Move;
    public BlockPooler BlockPooler;
    public float WaitSecondGameOver;
    public bool GameOverBool=false;
    public BlocksSaveDataList BlocksSaveDataList;

    IEnumerator CreateWaitBlock()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < SpawnRate; i++)
        {
            CreateRandomBlock();
        }
    }

    IEnumerator GameWaitOver()
    {
        GameOverBool = true;
        yield return new WaitForSeconds(3);
        EventManager.Instance.GameOverAll();
    }
    
    private void Start()
    {
        EventManager.Instance.Systems.Add(this);
        Size = EventManager.Instance.Size;
        Delay = EventManager.Instance.SizeBox / Size;
        BlockEmptyPref=EventManager.Instance.BlockEmptyPref;
        SpawnRate = EventManager.Instance.SpawnRate;
        StopRateBase = EventManager.Instance.StopRateBase;
        // BlockPooler = GetComponent<BlockPooler>();
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
                    Blocks[x, y, z] = null;
                    GameObject emptyblock = Instantiate(BlockEmptyPref, transform);
                    emptyblock.transform.localPosition = new Vector3(x * Delay, y * Delay, z * Delay);
                }
            }
        }
        // LoadGame();
        // StartCoroutine(CreateWaitBlock());
        Invoke("LoadGame",0.1f);
        // CreateRandomBlock();
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
        GameOverCheck();
        // Debug.Log(GameOverCheck());
        return false;
    }
    
    private GameObject CreateBlock(int x, int y, int z)
    {
        if (CheckFull())
        {
            // BlockPooler = GetComponent<BlockPooler>();
            if (BlockPooler != null) // Check if BlockPooler is not null
            {
                GameObject block = BlockPooler.GetBlock();
                if (block != null) // Check if block is not null after getting from the pool
                {
                    block.SetActive(true);
                    block.transform.localPosition = new Vector3(x * Delay, y * Delay, z * Delay);
                    return block;
                }
                else
                {
                    Debug.LogError("Block retrieved from pool is null.");
                    return null;
                }
            }
            else
            {
                Debug.LogError("BlockPooler is not assigned.",gameObject);
                return null;
            }
        }
        else
        {
            Debug.LogError("CheckFull() returned false, cannot create block.");
            return null;
        }
    }

    private bool GameOverCheck()
    {
        for (int x = 1; x < Size-1; x++)
        {
            for (int y = 1; y < Size-1; y++)
            {
                for (int z = 1; z < Size-1; z++)
                {
                    if (Blocks[x, y, z] == null) return false;
                    if (Blocks[x,y,z].BlockModule.Stop) continue;
                    bool GameOverBoolCheck = MoveCheckGameOver(x,y,z);
                    if (GameOverBoolCheck) return false;
                }
            }
        }
//
        if(!GameOverBool)StartCoroutine(GameWaitOver());
        return true;
    }
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
            Blocks[x, y, z].BlockModule.RateStop = StopRateBase;
            Move += Blocks[x, y, z].BlockModule.MoveDec;
            return block;
        }

        return null;
    }
    private void MoveBlocks(int x1,int y1,int z1)
    {
        SaveGame(false);
        bool x_axi=x1!=0;
        bool y_axi=y1!=0;
        bool z_axi=z1!=0;
        bool x_nega = x1 < 0;
        bool y_nega = y1 < 0;
        bool z_nega = z1 < 0;
        for (int i = 0; i < Size - 1; i++)
        {
            if (x_axi) MoveX(x1, y1, z1, x_nega);
            else if (y_axi) MoveY(x1, y1, z1, y_nega);
            else if (z_axi) MoveZ(x1, y1, z1, z_nega);
        }
        Move?.Invoke();
        for (int i = 0; i < SpawnRate; i++)
        {
            CreateRandomBlock();
        }
    }
    private bool Check(int x1, int x)
    {
        return (x + x1 < Size - 1 && x + x1 > 0);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_z"></param>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="z1"></param>
    /// <returns></returns>
    private bool MoveCheck(int _x,int _y,int _z,int x1, int y1, int z1)
    {
        return Blocks[_x, _y, _z] != null && (Check(x1, _x) && Check(y1, _y) && Check(z1, _z));
    }
    private bool MoveCheckContinue(int _x,int _y,int _z,int x1, int y1, int z1)
    {
        if (!MoveCheck(_x, _y, _z, x1, y1, z1)) return false;
        return Blocks[_x + x1, _y + y1, _z + z1]==null||(!Blocks[_x + x1, _y + y1, _z + z1].BlockModule.Stop &&
                                                   (Blocks[_x + x1, _y + y1, _z + z1].BlockModule.Num == Blocks[_x, _y, _z].BlockModule.Num));
    }
    private bool MoveCheckGameOver(int _x, int _y, int _z)
    {
        // bool a0=MoveCheckContinue(_x, _y, _z, 0, 0, 0);
        bool a1=MoveCheckContinue(_x, _y, _z, 1, 0, 0);
        bool a2=MoveCheckContinue(_x, _y, _z, -1, 0, 0);
        bool a3=MoveCheckContinue(_x, _y, _z, 0, 1, 0);
        bool a4=MoveCheckContinue(_x, _y, _z, 0, -1, 0);
        bool a5=MoveCheckContinue(_x, _y, _z, 0, 0, 1);
        bool a6=MoveCheckContinue(_x, _y, _z, 0, 0, -1);
        return a1 || a2 || a3 || a4 || a5 || a6;
    }
    
    /// <summary>s
    /// 
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="z1"></param>
    /// <param name="negative"></param>
    private void MoveX(int x1, int y1, int z1, bool negative)
    {
        negative = !negative;
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
                    if (negative) _y = Size - 1 - y;
                    
                    if (MoveCheck(_x,_y,_z,x1,y1,z1))
                    {
                        if (Blocks[_x + x1, _y + y1, _z + z1] == null)
                        {
                            Blocks[_x + x1, _y + y1, _z + z1] = Blocks[_x, _y, _z];
                            Blocks[_x,_y,_z].BlockModule.MoveTo(_x + x1, _y + y1, _z + z1,Delay);
                            Blocks[_x, _y, _z] = null;
                        }
                        else if(!Blocks[_x, _y, _z].BlockModule.Stop&&!Blocks[_x + x1, _y + y1, _z + z1].BlockModule.Stop&&Blocks[_x + x1, _y + y1, _z + z1].BlockModule.Num==Blocks[_x,_y,_z].BlockModule.Num)
                        {
                            Blocks[_x, _y, _z].BlockModule.MoveTo(_x + x1, _y + y1, _z + z1,Delay,true);
                            Blocks[_x + x1, _y + y1, _z + z1].BlockModule.NumIcr();
                            Blocks[_x, _y, _z] = null;
                            // Destroy(Blocks[_x, _y, _z].gameObject);
                        }
                    }
                }
            }
        }
    }
    private void MoveY(int x1, int y1, int z1, bool negative)
    {
        negative = !negative;
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
                        else if(!Blocks[_x, _y, _z].BlockModule.Stop&&!Blocks[_x + x1, _y + y1, _z + z1].BlockModule.Stop&&Blocks[_x + x1, _y + y1, _z + z1].BlockModule.Num==Blocks[_x,_y,_z].BlockModule.Num)
                        {
                            Blocks[_x, _y, _z].BlockModule.MoveTo(_x + x1, _y + y1, _z + z1,Delay,true);
                            Blocks[_x + x1, _y + y1, _z + z1].BlockModule.NumIcr();
                            // Destroy(Blocks[_x, _y, _z].gameObject);
                            Blocks[_x, _y, _z] = null;
                        }
                    }
                }
            }
        }
    }
    private void MoveZ(int x1, int y1, int z1, bool negative)
    {
        negative = !negative;
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
                    
                    if (MoveCheck(_x,_y,_z,x1,y1,z1))
                    {
                        if (Blocks[_x + x1, _y + y1, _z + z1] == null)
                        {
                            Blocks[_x + x1, _y + y1, _z + z1] = Blocks[_x, _y, _z];
                            Blocks[_x,_y,_z].BlockModule.MoveTo(_x + x1, _y + y1, _z + z1,Delay);
                            Blocks[_x, _y, _z] = null;
                        }
                        else if(!Blocks[_x, _y, _z].BlockModule.Stop&&!Blocks[_x + x1, _y + y1, _z + z1].BlockModule.Stop&&Blocks[_x + x1, _y + y1, _z + z1].BlockModule.Num==Blocks[_x,_y,_z].BlockModule.Num)
                        {
                            Blocks[_x, _y, _z].BlockModule.MoveTo(_x + x1, _y + y1, _z + z1,Delay,true);
                            Blocks[_x + x1, _y + y1, _z + z1].BlockModule.NumIcr();
                            Blocks[_x, _y, _z] = null;
                            // Destroy(Blocks[_x, _y, _z].gameObject);
                        }
                    }
                }
            }
        }
    }

    public void Continue()
    {
        GameOverBool = false;
        for (int i = 1; i < Size-1; i++)
        {
            for (int j = 1; j < Size-1; j++)
            {
                for (int k = 1; k < Size-1; k++)
                {
                    Blocks[i, j, k] = null;
                }
            }
        }
        StartCoroutine(CreateWaitBlock());
    }

    private void OnDestroy()
    {
        Arrow.MoveBlock -= MoveBlocks;
    }

    public void GameOver()
    {
        SaveGame(true);
        return;
    }

    public void Restart()
    {
        Continue();
    }

    public void LoadGame()
    {
        global::BlocksSaveDataList blocksSaveDataList = Load();
        if(blocksSaveDataList.restart) EventManager.Instance.GameOverAll();
        if (blocksSaveDataList == null)
        {
            return;
        }
        for (int i = 0; i < blocksSaveDataList.BlockSaveDatas.Count; i++)
        {
            int i1 = blocksSaveDataList.BlockSaveDatas[i].x;
            int j1 = blocksSaveDataList.BlockSaveDatas[i].y;
            int k1 = blocksSaveDataList.BlockSaveDatas[i].z;
            int moveIndex = blocksSaveDataList.BlockSaveDatas[i].moveIndex;
            int num = blocksSaveDataList.BlockSaveDatas[i].num;
            bool stop = blocksSaveDataList.BlockSaveDatas[i].stop;
            GameObject block = CreateBlock(i1, j1, k1);
            Blocks[i1, j1, k1] = block.GetComponent<Block>();
            Blocks[i1, j1, k1].BlockModule.RateStop = StopRateBase;
            Move += Blocks[i1, j1, k1].BlockModule.MoveDec;
            Blocks[i1, j1, k1].BlockModule.MoveIndex = moveIndex;
            Blocks[i1, j1, k1].BlockModule.Num = num;
            Blocks[i1, j1, k1].BlockModule.Stop = stop;
            Blocks[i1, j1, k1].BlockView._BlockAnimation.Shake(Blocks[i1, j1, k1].transform,(float)moveIndex/(float)StopRateBase);
            Blocks[i1, j1, k1].BlockView.IncreaseView(num);
            if (stop) {Blocks[i1, j1, k1].BlockView.DisableView(); }
        }
        //
        EventManager.Instance.HPCur = blocksSaveDataList.hp;
        EventManager.Instance.Score = blocksSaveDataList.score;
    }

    public void SaveGame(bool restart)
    {
        List<BlockSaveData> blockSaveDatas = new List<BlockSaveData>();
        // BlocksSaveDataList = new BlocksSaveDataList();
        for (int i = 1; i < Size-1; i++)
        {
            for (int j = 1; j < Size-1; j++)
            {
                for (int k = 1; k < Size-1; k++)
                {
                    if (Blocks[i, j, k] != null)
                    {
                        blockSaveDatas.Add(new BlockSaveData(i,j,k,Blocks[i,j,k].BlockModule.MoveIndex,Blocks[i,j,k].BlockModule.Stop,Blocks[i,j,k].BlockModule.Num));
                    }
                }
            }
        }
        BlocksSaveDataList = new BlocksSaveDataList(blockSaveDatas,EventManager.Instance.Score,EventManager.Instance.HPCur,restart);
        Save(BlocksSaveDataList);
    }
    
    public static void Save(BlocksSaveDataList blocksSaveDataList)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/blocs.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        global::BlocksSaveDataList data=blocksSaveDataList;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    // Function to load the player's score
    public static BlocksSaveDataList Load()
    {
        string path = Application.persistentDataPath + "/blocs.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            BlocksSaveDataList data = formatter.Deserialize(stream) as BlocksSaveDataList;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
            return null;
        }
    }
}


[System.Serializable]
public class BlockSaveData
{
    public int x;
    public int y;
    public int z;
    [FormerlySerializedAs("freeze")] public int moveIndex;
    public bool stop;
    public int num;

    public BlockSaveData( int x,int y,int z,int moveIndex,bool stop,int num)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.moveIndex = moveIndex;
        this.stop = stop;
        this.num = num;
    }
}

[System.Serializable]

public class BlocksSaveDataList
{
    public List<BlockSaveData> BlockSaveDatas;
    public int score;
    public int hp;
    public bool restart;

    public BlocksSaveDataList(List<BlockSaveData> blockSaveDatas,int score,int hp,bool restart)
    {
        this.restart = restart;
        this.hp = hp;
        this.score = score;
        BlockSaveDatas = blockSaveDatas;
    }
}