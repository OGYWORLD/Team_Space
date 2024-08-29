using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTankerAction : MonoBehaviour
{
    public GameObject mainCamera; // ĳ���� ���� ī�޶�
    public Transform shieldTran; // �溮 Ʈ������(scale ���� �� �ʿ�)
    public Transform fireSpwanTrans; // ȭ����Ÿ�� ȭ�� ������ ��ġ

    private float speed = 5f; // ���ǵ�
    private Animator animator; // ĳ���� �ִϸ�����

    private bool isSetCameraBack; // �溮 ��, ī�޶� �ڷ� �̵��ߴ���
    private bool isPushedShieldKey; // �溮Ű (��Ŭ)�� ���ȴ���

    private Vector3 shieldScale = new Vector3(7.5f, 2f, 1f); // �溮 ��, �溮 ũ�� ����

    private int posNum = 3;
    private Vector3[] posForRay = new Vector3[3]; // �� �浹 ������ ���� ĳ���� ������ ����Ʈ
    private float detectRange = 1f; // �� Ž�� ����

    private CFirePooling firePooling; // ȭ����Ÿ �� ������Ʈ Ǯ�� ��ũ��Ʈ
    private CFireCoolTime fireCoolTime; // ȭ����Ÿ ��Ÿ�� UI ��ũ��Ʈ

    private bool isGoing = false; // ���� ����
    private float goingSpeed = 15f; // ���� �ӵ�

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

    private void Move() // �����¿� �̵�
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if(x != 0f || z != 0f) // �ȴ� �ִϸ��̼� ����
        {
            animator?.SetBool("isWalk", true);
        }
        else
        {
            animator?.SetBool("isWalk", false);
        }

        if (IsDetectWall(transform.forward)) // ���� �浹 �˻�
        {
            if(z > 0f)
            {
                z = 0f;
            }
        }
        if (IsDetectWall(-transform.forward)) // �ĸ� �浹 �˻�
        {
            if (z < 0f)
            {
                z = 0f;
            }
        }
        if (IsDetectWall(-transform.right)) // ���� �浹 �˻�
        {
            if (x < 0f)
            {
                x = 0f;
            }
        }
        if (IsDetectWall(transform.right)) // ���� �浹 �˻�
        {
            if (x > 0f)
            {
                x = 0f;
            }
        }

        Vector3 direction = new Vector3(x, 0, z);

        // ���͸� ����ȭ�Ͽ� ũ�Ⱑ �׻� 1�� �ǵ��� ��
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }

        transform.position += direction * speed * Time.deltaTime;
    }

    private void Jump() // ����
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }

    private void HammerAttack() // ��Ŀ ���� ���� - ��ġ �ֵθ���
    {
        if(Input.GetMouseButtonDown(0))
        {
            animator?.SetBool("isHammerAttack", true);

            StartCoroutine(HammerAttackAnim());
        }
    }

    private void Shield() // ��Ŀ ��Ŭ - �溮
    {
        if(Input.GetMouseButton(1)) // ��Ŭ�� ������ ��
        {
            isPushedShieldKey = true;

            shieldTran.localScale = shieldScale; // �溮 ������ ����

            animator?.SetBool("isShield", true); // �ִϸ��̼� ����

            if(!isSetCameraBack) // ī�޶� �ڷ� ������ �ڷ�ƾ�� ���� ���� �ƴ϶��
                StartCoroutine(ShieldBackCamera()); // ī�޶� �ڷ� ������
        }

        if(Input.GetMouseButtonUp(1)) // ��Ŭ�� ���� ��
        {
            isPushedShieldKey = false;

            shieldTran.localScale = new Vector3(1f, 1f, 1f);

            animator?.SetBool("isShield", false); // �ִϸ��̼� ����

            StartCoroutine(ShieldFrontCamera()); // ī�޶� ������ ��������
        }
    }

    private bool IsDetectWall(Vector3 dir) // �� �浹 �˻�
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

    private void FireAttack() // E��ų - ȭ����Ÿ
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

    private void Go() // ��Shift ��ų - ����
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator?.SetBool("isGo", true);
            isGoing = true;
        }
    }

    private void Ground() // �ñر� Q - �����м�
    {
        // TODO: ����� ������ ���� Ŭ���� �ϳ� ���� �ű⼭ �ñر� count
        if(Input.GetKeyDown(KeyCode.Q))
        {
            animator?.SetBool("isGround", true);

            StartCoroutine(GroundAnim());
        }
    }

    private IEnumerator HammerAttackAnim() // ���� ���� �ִϸ��̼� �ڷ�ƾ
    {
        yield return new WaitForSeconds(1f);

        animator?.SetBool("isHammerAttack", false);
    }

    private IEnumerator ShieldBackCamera() // �溮 ī�޶� �ڷ� �̵� �ڷ�ƾ
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

    private IEnumerator ShieldFrontCamera() // �溮 ī�޶� ������ �̵� �ڷ�ƾ
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

    private IEnumerator FireAnim() // E��ų - ȭ����Ÿ �ִϸ��̼� �ڷ�ƾ
    {
        yield return new WaitForSeconds(1f);

        animator?.SetBool("isFire", false);
    }

    private IEnumerator GroundAnim() // Q �ñر� - �����м� �ִϸ��̼� �ڷ�ƾ
    {
        yield return new WaitForSeconds(1f);

        animator?.SetBool("isGround", false);
    }

    private IEnumerator JumpAnim() // ���� �ִϸ��̼� �� ���� �ڷ�ƾ
    {
        float sumTime = 0f;
        float totalTime = 0.5f;

        Vector3 initPos = transform.position;
        Vector3 jumpPos = new Vector3(initPos.x, initPos.y + 1f, initPos.z);

        while(sumTime < totalTime) // �ö󰡱�
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(x, 0, z);

            // ���͸� ����ȭ�Ͽ� ũ�Ⱑ �׻� 1�� �ǵ��� ��
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

        while (sumTime < totalTime) // ��������
        {
            float t = sumTime / totalTime;
            transform.position = Vector3.Lerp(jumpPos, initPos, t);

            sumTime += Time.deltaTime;

            yield return null;
        }

        yield break;
    }
}
