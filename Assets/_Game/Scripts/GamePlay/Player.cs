using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// None: la de nguoi choi neu vuot ngan qua thi se tra ve none
public enum Direct { Forward, Back, Right, Left, None }

public class Player : MonoBehaviour
{
    public LayerMask layerBrick;
    public float speed = 5;
    public Transform brickHolder;
    public Transform playerBrickPrefab;
    public Transform playerSkin;
    


    private Vector3 mouseDown, mouseUp;
    private bool isMoving;
    private bool isControl;
    private Vector3 moveNextPoint;
    private List<Transform> playerBricks = new List<Transform>();   


    private void Start()
    {
        // OnInit();

        ClearBrick();
        playerSkin.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Set di chuyen
        if (GameManager.Instance.IsState(GameState.Gameplay) && !isMoving)
        {
            // khi vuot chuot len
            if (Input.GetMouseButtonDown(0) && !isControl)
            {
                isControl = true;
                mouseDown = Input.mousePosition;
            }
            // khi vuot chuot xuong
            if (Input.GetMouseButtonUp(0) && isControl)
            {
                isControl = false;
                mouseUp = Input.mousePosition;

                Direct direct = GetDirect(mouseDown, mouseUp);
                if (direct != Direct.None)
                {
                    moveNextPoint = GetNextPoint(direct);
                    isMoving = true;
                }
                // Test kiem tra xem co di chuyen dung huong hay khong
                // Debug.Log(GetDirect(mouseDown, mouseUp));

                // Get huong roi Get vi tri tiep theo
                // Debug.Log(GetNextPoint(GetDirect(mouseDown, mouseUp)));
            }
        }
        else if (isMoving)
        {
            if (Vector3.Distance(transform.position, moveNextPoint) < 0.1f)
            {
                isMoving = false;
            }
            transform.position = Vector3.MoveTowards(transform.position, moveNextPoint, Time.deltaTime * speed);
        }
        
        
    }

    // tra ve direct
    private Direct GetDirect(Vector3 mouseDown, Vector3 mouseUp)
    {
        Direct direct = Direct.None;
        // de xem nguoi choi vuot theo chieu nao
        float deltaX = mouseUp.x - mouseDown.x;
        float deltaY = mouseUp.y - mouseDown.y;
        // check vuot
        // kiem tra khoang cach
        if (Vector3.Distance(mouseDown, mouseUp) < 80)
        {
            direct = Direct.None;
        }
        else
        {
            // check len xuong
            if (Mathf.Abs(deltaY) > Mathf.Abs(deltaX))
            {
                // vuot len tren
                if (deltaY > 0)
                {
                    direct = Direct.Forward;
                }
                // vuot xuong duoi
                else
                {
                    direct = Direct.Back;
                }
            }
            // checl trai phai
            else
            {
                // vuot ben phai
                if (deltaX > 0)
                {
                    direct = Direct.Right;
                }
                // vuot sang trai
                else
                {
                    direct = Direct.Left;
                }
            }
        }

        return direct;
    }

    //private Vector3 GetNextPoint(Direct direct)
    //{
    //    // ban raycast
    //    RaycastHit hit;
    //    //Bat dau tu truoc mat thi can 1 vector3
    //    // tranform.position la vi tri cua nhan vat
    //    Vector3 nextPoint = transform.position;
    //    Vector3 dir = Vector3.zero;

    //    // check moi huong di chuyen
    //    switch (direct)
    //    {

    //        case Direct.Forward:
    //            dir = Vector3.forward;
    //            break;
    //        case Direct.Back:
    //            dir = Vector3.back;
    //            break;
    //        case Direct.Left:
    //            dir = Vector3.left;
    //            break;
    //        case Direct.Right:
    //            dir = Vector3.right;
    //            break;
    //        case Direct.None:

    //            break;

    //        default:
    //        break;
    //    }

    //    // su dung vong lap tiep theo
    //    for (int i = 1; i < 100; i++)
    //    {
    //        // Debug de kiem tra tia raycast
    //        // Debug.LogError(transform.position + dir * i);
    //        // ban tia raycast
    //        // thay Vector3.foward thanh dir
    //        // khi khong check duoc vi tri tia raycast ban ra laf do no o giua tam cua box
    //        // nen chung ta can tang goc bang tia raycast nen 1 chut bang cach: cho transform.position + dir * i + Vector3.up bang cach ( + them + Vector3.up)
    //        if (Physics.Raycast(transform.position + dir * i + Vector3.up, Vector3.down, out hit, 10f, layerBrick))
    //        {
    //            nextPoint = hit.collider.transform.position;
    //        }
    //        else
    //        // neu ko co thi tra ve break
    //        {
    //            break;
    //        }
    //    }

    //    return nextPoint;


    //}


    private Vector3 GetNextPoint(Direct direct)
    {
        // Ban raycast
        RaycastHit hit;
        Vector3 nextPoint = transform.position;
        Vector3 dir = Vector3.zero;

        // Check moi huong di chuyen
        switch (direct)
        {
            case Direct.Forward:
                dir = Vector3.forward;
                transform.rotation = Quaternion.Euler(0, 0, 0); // Xoay ra trước
                break;
            case Direct.Back:
                dir = Vector3.back;
                transform.rotation = Quaternion.Euler(0, 180, 0); // Xoay ra sau
                break;
            case Direct.Left:
                dir = Vector3.left;
                transform.rotation = Quaternion.Euler(0, -90, 0); // Xoay sang trái
                break;
            case Direct.Right:
                dir = Vector3.right;
                transform.rotation = Quaternion.Euler(0, 90, 0); // Xoay sang phải
                break;
            case Direct.None:
                break;
            default:
                break;
        }

        // Su dung vong lap tiep theo
        for (int i = 1; i < 100; i++)
        {
            // Ban tia raycast
            if (Physics.Raycast(transform.position + dir * i + Vector3.up, Vector3.down, out hit, 10f, layerBrick))
            {
                nextPoint = hit.collider.transform.position;
            }
            else
            {
                break;
            }
        }

        return nextPoint;
    }



    // khoi tao va reset chi so: OnInit
    public void OnInit()
    {
        ClearBrick();
        // dau tien khoi tao isMoving la true hay false
        isMoving = false;
        isControl = false;
        playerSkin.localPosition = Vector3.zero;
    }

    public void AddBrick()
    {
        // lay vi tri mong muon
        int index = playerBricks.Count;
        Transform playerBrick = Instantiate(playerBrickPrefab, brickHolder);
        playerBrick.localPosition = Vector3.down + index * 0.25f * Vector3.up;
        playerBricks.Add(playerBrick);
        // tang vi tri player len khi va cham vs brick
        playerSkin.localPosition = playerSkin.localPosition + Vector3.up * 0.25f;
    }

    public void RemoveBrick()
    {
        int index = playerBricks.Count - 1;
        if (index > 0)
        {
            Transform playerBrick = playerBricks[index];
            playerBricks.Remove(playerBrick);
            Destroy(playerBrick.gameObject);
            // giam vi tri player len khi va cham vs brick
            playerSkin.localPosition = playerSkin.localPosition - Vector3.up * 0.25f;
        }
    }

    public void ClearBrick()
    {
        for (int i = 0; i < playerBricks.Count; i++)
        {
            // chi xoa di nhung phan tu van con do
            Destroy(playerBricks[i].gameObject);
        }

        playerBricks.Clear();
    }
}
