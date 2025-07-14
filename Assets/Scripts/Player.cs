using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NewBehaviourScript : MonoBehaviour
{
    const float half = 0.5f;

    [field: SerializeField, Header("방향")]
    public Vector3 dir { get; set; }
    [field: SerializeField]
    public float moveSpeed { get; set; } = 5.0f;
    public int hp = 100;

    public float tempTextureUpdateTime { get; set; }
    [field: SerializeField, Header("애니메이션 동작 간격")]
    public float textureUpdateTime { get; set; } = 2.0f;
    public int textureNum { get; set; }
    [field: SerializeField, Header("유닛의 애니메이션을 위한 텍스쳐")]
    public Texture2D[] textures { get; set; }
    public GameObject childPlayer { get; set; }
    public Vector3 centerVector { get; set; }
    public Vector3 lookVector { get; set; }
    public Vector3 lookDir { get; set; }
    public float fireTime { get; } = 0.1f;
    public float tempFireTime { get; set; } = 0.1f;

    //public Joystick leftJoystick { get; set; }
    //public Joystick rightJoystick { get; set; }
    [field: SerializeField, Header("여기에 총알 프리팹을 넣어주세요")]
    public GameObject bullet { get; set; }
    public Transform bulletPoint { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        childPlayer = this.transform.Find("chilPlayer").gameObject;
        bulletPoint = this.transform.Find("bullletPoint");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                Debug.Log(dir);
                AnimMove();
            }
            else
            {
                dir = new Vector3(0, 0, 0);
            }
            //중간지점 찾기(플레이어의 위치)
            centerVector = new Vector3(Screen.width * half, 0, Screen.height * half);
            //마우스 위치 값
            lookVector = Input.mousePosition;
            //바라볼 위치
            lookVector = new Vector3(lookVector.x, 0, lookVector.y);

            lookDir = lookVector - centerVector;
            Fire();
        }
        this.transform.Translate(dir * moveSpeed * Time.smoothDeltaTime);
        this.transform.rotation = Quaternion.LookRotation(lookDir);
    }
    //움직이는 애니메이션 Texture 변경
    void AnimMove()
    {
        if(tempTextureUpdateTime > textureUpdateTime)
        {
            tempTextureUpdateTime = 0;
            textureNum++;
            if(textureNum > textures.Length - 1)
            {
                textureNum = 0;
            }
            childPlayer.GetComponent<MeshRenderer>().material.mainTexture =
                textures[textureNum];
        }
        else
        {
            tempTextureUpdateTime += Time.deltaTime;
        }
    }
    void Fire()
    {
        if (Input.GetMouseButton(0))
        {
            if(tempFireTime > fireTime)
            {
                tempFireTime = 0;
                GameObject tepBullet = Instantiate(bullet, bulletPoint.position,
                    Quaternion.LookRotation(lookDir));
            }
            else if(tempFireTime < fireTime)
            {
                    tempFireTime += Time.deltaTime;
            }
        }
    }

    public void Hit(int damge)
    {
        hp -= damge;

        if(hp < 0)
        {
            Time.timeScale = 0;
        }
    }
}