using UnityEngine;

public class roroCamera : MonoBehaviour
{
    public Transform roroTransform; // roro의 Transform
    public float followSpeed = 0.2f; // 카메라의 이동 속도 (기본값 0.2f)
    public Vector2 center; // 카메라의 클램핑 중심
    public Vector2 size; // 카메라 클램핑 영역의 크기
    private float height;
    private float width;
    private Vector3 velocity = Vector3.zero; // SmoothDamp에 사용될 속도 벡터
    public float maxDistance = 5f; // 카메라와 roro 사이의 최대 거리

    void Start()
    {
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0f); // 연한 빨간색 (알파값 0.5)
        Gizmos.DrawCube(center, size);
    }

    void LateUpdate()
    {
        if (roroTransform != null)
        {
            // roro의 위치에 따라 카메라의 목표 위치 계산 (화면 크기와 관계없이 roro를 바로 따라감)
            Vector3 targetPosition = new Vector3(roroTransform.position.x, roroTransform.position.y, -10f);

            // 카메라가 roro와의 거리가 너무 멀어지지 않도록 제한
            float distance = Vector3.Distance(transform.position, targetPosition);
            if (distance > maxDistance)
            {
                // roro와의 거리가 maxDistance 이상 벌어지면 카메라가 제한된 거리만큼만 따라감
                targetPosition = transform.position + (targetPosition - transform.position).normalized * maxDistance;
            }

            // 카메라가 부드럽게 roro를 따라가도록 SmoothDamp 사용
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followSpeed);

            // 카메라 클램핑 (화면 밖으로 나가지 않도록 제어)
            float lx = size.x * 0.5f - width;
            float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

            float ly = size.y * 0.5f - height;
            float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);

            // 최종 카메라 위치 설정
            transform.position = new Vector3(clampX, clampY, -10f);
        }
    }
}
