using UnityEngine;
using System.Collections;
using System;

public class Arrow : MonoBehaviour
{
    private Vector3 Move;
    public bool x;
    public bool y;
    public bool z;
    public bool flip;
    private EventManager eventManager;
    [SerializeField] private float reload;
    private bool reloading = true;
    [HideInInspector] public static Action<int, int, int> MoveBlock;

    // Флаг, определяющий, активирован ли режим двойного клика
    public bool doubleClick = true;

    private void Start()
    {
        reload = EventManager.Instance.Speed;
        eventManager = EventManager.Instance;
        doubleClick = EventManager.Instance.doubleClick;
        EventManager.Instance.doubleClickCheck += ChangeDouble;
        if (x) Move = transform.right;
        else if (y) Move = transform.up;
        else if (z) Move = transform.forward;
        if (flip) Move = -Move;
    }

    private void OnMouseDown()
    {
        if (Time.timeScale == 0f) return;
        if (!reloading) return;
    
        // Проверяем, была ли кнопка мыши нажата в текущем кадре
        if (!Input.GetMouseButtonDown(0)) return;

        // Если режим двойного клика активирован и время между кликами мало, то выполняем действие
        if (doubleClick && Time.time < lastClickTime + doubleClickDelay)
        {
            MoveBlock?.Invoke((int)Move.x, (int)Move.y, (int)Move.z);
            StartCoroutine(ReloadDelay());
        }
        if (!doubleClick)
        {
            MoveBlock?.Invoke((int)Move.x, (int)Move.y, (int)Move.z);
            StartCoroutine(ReloadDelay());
        }
        lastClickTime = Time.time;
    }


    // Переменные для отслеживания последнего времени клика и задержки между кликами
    private float lastClickTime;
    public float doubleClickDelay = 0.3f;

    IEnumerator ReloadDelay()
    {
        reloading = false;
        yield return new WaitForSeconds(reload);
        reloading = true;
    }

    public void ChangeDouble(bool data)
    {
        doubleClick = data;
    }
}