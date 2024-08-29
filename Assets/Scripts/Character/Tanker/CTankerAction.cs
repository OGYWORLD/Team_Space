using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTankerAction : MonoBehaviour
{
    public GameObject mainCamera; // 캐릭터 메인 카메라
    public Transform shieldTran; // 방벽 트랜스폼(scale 조정 시 필요)
    public Transform fireSpwanTrans; // 화염강타의 화염 리스폰 위치

    private float speed = 5f; // 스피드
    private Animator animator; // 캐릭터 애니메이터

    private bool isSetCameraBack; // 방벽 시, 카메라가 뒤로 이동했는지
    private bool isPushedShieldKey; // 방벽키 (우클)이 눌렸는지

    private Vector3 shieldScale = new Vector3(7.5f, 2f, 1f); // 방벽 시, 방벽 크기 조정

    private int posNum = 3;
    private Vector3[] posForRay = new Vector3[3]; // 벽 충돌 감지를 위한 캐릭터 포지션 리스트
    private float detectRange = 1f; // 벽 탐색 범위

    private CFirePooling firePooling; // 화염강타 불 오브젝트 풀링 스크립트
    private CFireCoolTime fireCoolTime; // 화염강타 쿨타임 UI 스크립트

    private bool isGoing = false; // 돌진 여부
    private float goingSpeed = 15f; // 돌진 속도

    private void Start()
    {
        animator = GetComponent<Animator>();
        firePooling = GetComponent<CFirePooling>();
        fireCoolTime = GetComponent<CFireCoolTime>();
    }

    private void Update()
    {
        if(!isGoing)
        {
            Move();
            HammerAttack();
            Shield();
            FireAttack();
            Ground();
            Jump();
        }
        Go();
        if(isGoing)
        {
            transform.position += new Vector3(0f, 0f, goingSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall"))
        {
            isGoing = false;
            animator?.SetBool("isGo", false);
        }
    }

    private void Move() // 상하좌우 이동
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if(x != 0f || z != 0f) // 걷는 애니메이션 설정
        {
            animator?.SetBool("isWalk", true);
        }
        else
        {
            animator?.SetBool("isWalk", false);
        }

        if (IsDetectWall(transform.forward)) // 정면 충돌 검사
        {
            if(z > 0f)
            {
                z = 0f;
            }
        }
        if (IsDetectWall(-transform.forward)) // 후면 충돌 검사
        {
            if (z < 0f)
            {
                z = 0f;
            }
        }
        if (IsDetectWall(-transform.right)) // 좌측 충돌 검사
        {
            if (x < 0f)
            {
                x = 0f;
            }
        }
        if (IsDetectWall(transform.right)) // 우측 충돌 검사
        {
            if (x > 0f)
            {
                x = 0f;
            }
        }

        Vector3 direction = new Vector3(x, 0, z);

        // 벡터를 정규화하여 크기가 항상 1이 되도록 함
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }

        transform.position += direction * speed * Time.deltaTime;
    }

    private void Jump() // 점프
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }

    private void HammerAttack() // 탱커 근접 공격 - 망치 휘두르기
    {
        if(Input.GetMouseButtonDown(0))
        {
            animator?.SetBool("isHammerAttack", true);

            StartCoroutine(HammerAttackAnim());
        }
    }

    private void Shield() // 탱커 우클 - 방벽
    {
        if(Input.GetMouseButton(1)) // 우클을 눌렀을 때
        {
            isPushedShieldKey = true;

            shieldTran.localScale = shieldScale; // 방벽 스케일 조정

            animator?.SetBool("isShield", true); // 애니메이션 실행

            if(!isSetCameraBack) // 카메라를 뒤로 보내는 코루틴이 실행 중이 아니라면
                StartCoroutine(ShieldBackCamera()); // 카메라 뒤로 보내기
        }

        if(Input.GetMouseButtonUp(1)) // 우클을 땠을 때
        {
            isPushedShieldKey = false;

            shieldTran.localScale = new Vector3(1f, 1f, 1f);

            animator?.SetBool("isShield", false); // 애니메이션 실행

            StartCoroutine(ShieldFrontCamera()); // 카메라 앞으로 가져오기
        }
    }

    private bool IsDetectWall(Vector3 dir) // 벽 충돌 검사
    {
        posForRay[0] = transform.position;
        posForRay[1] = transform.position + new Vector3(0f, 1f, 0f);
        posForRay[2] = transform.position + new Vector3(0f, 2f, 0f);

        foreach (Vector3 pos in posForRay)
        {
            Debug.DrawRay(pos, dir * detectRange, Color.red);

            if (Physics.Raycast(pos, dir, out RaycastHit hit, detectRange))
            {
                if(hit.collider.CompareTag("Wall"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void FireAttack() // E스킬 - 화염강타
    {
        if(Input.GetKeyDown(KeyCode.E) && fireCoolTime.isCoolTime)
        {
            fireCoolTime.StartCoolTime();

            firePooling.pool[firePooling.poolIdx].transform.position = fireSpwanTrans.position;
            firePooling.pool[firePooling.poolIdx].SetActive(true);
            firePooling.poolIdx++;

            if(firePooling.poolIdx >= firePooling.pool.Count)
            {
                firePooling.poolIdx = 0;
            }

            animator?.SetBool("isFire", true);

            StartCoroutine(FireAnim());
        }
    }

    private void Go() // 좌Shift 스킬 - 돌진
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator?.SetBool("isGo", true);
            isGoing = true;
        }
    }

    private void Ground() // 궁극기 Q - 대지분쇄
    {
        // TODO: 사용자 정보를 담은 클래스 하나 만들어서 거기서 궁극기 count
        if(Input.GetKeyDown(KeyCode.Q))
        {
            animator?.SetBool("isGround", true);

            StartCoroutine(GroundAnim());
        }
    }

    private IEnumerator HammerAttackAnim() // 근접 공격 애니메이션 코루틴
    {
        yield return new WaitForSeconds(1f);

        animator?.SetBool("isHammerAttack", false);
    }

    private IEnumerator ShieldBackCamera() // 방벽 카메라 뒤로 이동 코루틴
    {
        isSetCameraBack = true;

        float sumTime = 0f;
        float totalTime = 1f;

        Vector3 targetCameraPos = new Vector3(mainCamera.transform.localPosition.x, mainCamera.transform.localPosition.y, -3f);

        while(sumTime <= totalTime)
        {
            float t = sumTime / totalTime;
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, targetCameraPos, t);

            sumTime += Time.deltaTime;

            if(!isPushedShieldKey)
            {
                break;
            }

            yield return null;
        }

        yield break;
    }

    private IEnumerator ShieldFrontCamera() // 방벽 카메라 앞으로 이동 코루틴
    {
        isSetCameraBack = false;

        float sumTime = 0f;
        float totalTime = 1f;

        Vector3 initCameraPos = new Vector3(mainCamera.transform.localPosition.x, mainCamera.transform.localPosition.y, 0.3f);

        while (sumTime <= totalTime)
        {
            float t = sumTime / totalTime;
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, initCameraPos, t);

            sumTime += Time.deltaTime;

            if (isPushedShieldKey)
            {
                break;
            }

            yield return null;
        }

        yield break;
    }

    private IEnumerator FireAnim() // E스킬 - 화염강타 애니메이션 코루틴
    {
        yield return new WaitForSeconds(1f);

        animator?.SetBool("isFire", false);
    }

    private IEnumerator GroundAnim() // Q 궁극기 - 대지분쇄 애니메이션 코루틴
    {
        yield return new WaitForSeconds(1f);

        animator?.SetBool("isGround", false);
    }

    private IEnumerator JumpAnim() // 점프 애니메이션 및 높이 코루틴
    {
        float sumTime = 0f;
        float totalTime = 0.5f;

        Vector3 initPos = transform.position;
        Vector3 jumpPos = new Vector3(initPos.x, initPos.y + 1f, initPos.z);

        while(sumTime < totalTime) // 올라가기
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(x, 0, z);

            // 벡터를 정규화하여 크기가 항상 1이 되도록 함
            if (direction.magnitude > 1)
            {
                direction.Normalize();
            }

            float t = sumTime / totalTime;
            transform.position = Vector3.Lerp(initPos, jumpPos, t);

            sumTime += Time.deltaTime;

            yield return null;
        }

        sumTime = 0f;

        while (sumTime < totalTime) // 내려가기
        {
            float t = sumTime / totalTime;
            transform.position = Vector3.Lerp(jumpPos, initPos, t);

            sumTime += Time.deltaTime;

            yield return null;
        }

        yield break;
    }
}
