using UnityEngine;


public class Block : MonoBehaviour
{
    [HideInInspector] public BlockView BlockView;
    [HideInInspector] public BlockModule BlockModule;
    [HideInInspector] public bool Used;

    private void Awake()
    {
        BlockView = GetComponent<BlockView>();
        BlockModule = GetComponent<BlockModule>();
    }
}
