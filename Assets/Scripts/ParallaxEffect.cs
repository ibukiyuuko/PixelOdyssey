using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    //��������
    public Camera cam;
    public Transform followTarget; //���follow�Ķ���
    //�Ӿ����ͬ��Ʒ�в�ͬcamera move����ͬ���ƶ������Ӿ���
    Vector2 startingPosition;//��ס���ݲ�����λ��

    float startingZ; //��Ʒ��һ�γ����ڳ���

    //ÿһ֡�ƶ���ֵ��ֻ���������xy�ƶ���ֵ
    //��ʼʱû���Ӿ�����ƶ���Խ��Խ���Ӿ��ÿ��project�в�һ�����ƶ�ֵ
    //��ʼʱû���Ӿ�����ƶ���Խ��Խ���Ӿ��ÿ��project�в�һ�����ƶ�ֵ
    Vector2 camMoveSinceStart => (Vector2) cam.transform.position - startingPosition;

    //����z��λ�þ����Ӿ������ӵĴ�С
    float zDistanceFormTarget => 
        transform.position.z - followTarget.transform.position.z;

    //����Ŀ������������Ǹ�����ʹ�ý�����Զ
    /*Clipping Planes[�и���]
    Near����ƽ�棬���������ܿ����Ķ�����
    Far��Զƽ�棬�������Զ�ܿ����Ķ�����*/
float clippingPlane => 
    (cam.transform.position.z + (zDistanceFormTarget > 0 ? cam.farClipPlane :  cam.nearClipPlane));

    //ÿ֡�ƶ���ֵ
    //Mathf.Abs(f)��������ֵ->���㲢����ָ������ f ����ֵ
float parallaxFactor => Mathf.Abs(zDistanceFormTarget) / clippingPlane;

// Start is called before the first frame update
void Start()
{
    startingPosition = transform.position;
    startingZ = transform.position.z; //�̶�z��
}

// Update is called once per frame
void Update()
{
    //���ݾ�ͷ�ƶ��ľ���ȥ�ƶ��Ӿ������
    Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;
    //ֻ��xy���̶�z
    transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
}
}
