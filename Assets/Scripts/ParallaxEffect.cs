using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    //两个引用
    public Camera cam;
    public Transform followTarget; //相机follow的对象
    //视觉差：不同物品有不同camera move，不同的移动创造视觉差
    Vector2 startingPosition;//记住数据差对象的位置

    float startingZ; //物品第一次出现在场景

    //每一帧移动的值，只计算相机在xy移动的值
    //开始时没有视觉差，在移动后越来越有视觉差，每个project有不一样的移动值
    //开始时没有视觉差，在移动后越来越有视觉差，每个project有不一样的移动值
    Vector2 camMoveSinceStart => (Vector2) cam.transform.position - startingPosition;

    //根据z轴位置决定视觉差因子的大小
    float zDistanceFormTarget => 
        transform.position.z - followTarget.transform.position.z;

    //根据目标距离是正还是负决定使用近还是远
    /*Clipping Planes[切割面]
    Near：近平面，摄像机最近能看到的东西。
    Far：远平面，摄像机最远能看到的东西。*/
float clippingPlane => 
    (cam.transform.position.z + (zDistanceFormTarget > 0 ? cam.farClipPlane :  cam.nearClipPlane));

    //每帧移动的值
    //Mathf.Abs(f)——绝对值->计算并返回指定参数 f 绝对值
float parallaxFactor => Mathf.Abs(zDistanceFormTarget) / clippingPlane;

// Start is called before the first frame update
void Start()
{
    startingPosition = transform.position;
    startingZ = transform.position.z; //固定z轴
}

// Update is called once per frame
void Update()
{
    //根据镜头移动的距离去移动视觉差对象
    Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;
    //只变xy，固定z
    transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
}
}
