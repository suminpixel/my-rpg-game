using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : MonoBehaviour
{
    public Grid _grid;

    Vector3Int _cellPos = Vector3Int.zero; 

    public float _speed = 5.0f;
    MoveDir _dir = MoveDir.None;
    bool _isMoving = false;

    Animator _animator;

    public MoveDir Dir { 
        get {
            return _dir;
        }
        set { 

            if(_dir == value){
                return;
            }

            switch(value){
                case MoveDir.Up :
                    _animator.Play("WALK_BACK");
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //방향 전환
                    break;
                case MoveDir.Down :
                _animator.Play("WALK_FRONT");
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //방향 전환
                    break;
                case MoveDir.Left :
                _animator.Play("WALK_RIGHT");
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f); //방향 전환
                    break;
                case MoveDir.Right :
                    _animator.Play("WALK_RIGHT");;
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //방향 전환
                    break;
                case MoveDir.None :
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //방향 전환
                    break;

                            
           }

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = getComponent<Animator>();
        Vector3 pos = _grid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f);
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        GetDirInput(); //key 입력 받기 
        UpdateIsMoving(); // 이동 가능한 상태일때 실제 좌표 이동
        UpdatePosition(); // 셀(칸) 단위 이동 처리 (클라이언트 뷰 에서 보이는것)
        
    }

    
    void UpdatePosition(){ 
        if(_isMoving == false){
            return;
        }
        Vector3 destPos = _grid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f);
        Vector3 moveDir = destPos - transform.position;
        

        // 도착여부 체크
        float dist = moveDir.magnitude;
        if(dist < _speed * Time.deltaTime){
            transform.position = destPos;
            _isMoving = false;
        
        }else{ //도착하지 않은경우 조금씩 조금씩 목적지로 이동
            transform.position += moveDir.normalized * _speed * Time.deltaTime;
            _isMoving = true;
        }
    }
    
    void UpdateIsMoving(){
        if(_isMoving == false){
           switch(Dir){
                case MoveDir.Up :
                    _cellPos += Vector3Int.up;
                    _isMoving = true;
                    break;
                case MoveDir.Down :
                    _cellPos += Vector3Int.down;
                    _isMoving = true;
                    break;
                case MoveDir.Left :
                    _cellPos += Vector3Int.left;
                    _isMoving = true;
                    break;
                case MoveDir.Right :
                    _cellPos += Vector3Int.right;
                    _isMoving = true;
                    break;
                            
           }
       }
    }
    void GetDirInput()
    {
        if(Input.GetKey(KeyCode.W)){
            //trasnform.position += Vector3.up * Time.deltaTime * _speed;
            Dir = MoveDir.Up;
        }else if(Input.GetKey(KeyCode.S)){
            Dir = MoveDir.Down;
        }else if(Input.GetKey(KeyCode.A)){
            Dir = MoveDir.Left;
        }else if(Input.GetKey(KeyCode.D)){
            Dir = MoveDir.Right;
        }else{

        }
    }
}
