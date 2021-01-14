using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track_Exercise : MonoBehaviour
{
    private string personPath = "Prefabs/SkelMesh_Bodyguard_03";   //人物模型地址
    private List<Vector3> wayPointsList = new List<Vector3>();     //存储所有轨迹点
    private float MoveSpeed = 0.08f;  // 移动速度
    private float RotateSpeed = 2f;   // 旋转速度
    float rotateProgress = 0;
    public int wayIndex = 0;          // 人物初始点位置 
    Quaternion targetQuaternion;      //目标旋转状态
    Quaternion lastQuaternion;        //人物当前旋转状态
    GameObject person;


    // Start is called before the first frame update
    void Start()
    {
        // 初始化轨迹点
        InitWayPoints();
        // 初始化人物模型 大小、位置
        person = (GameObject)Instantiate(Resources.Load(personPath));
        person.transform.localScale = new Vector3(Random.Range(0.058f, 0.063f), Random.Range(0.058f, 0.063f), Random.Range(0.058f, 0.063f));
        person.transform.position = wayPointsList[wayIndex];
        // 初始化人物模型朝向
        CalculateRotateData(wayIndex);
        person.transform.rotation = Quaternion.LookRotation(initDir);
    }

    // Update is called once per frame
    void Update()
    {
        // 移动
        Move();
    }

    // 随机生成轨迹点
    private void InitWayPoints()
    {
        wayPointsList.Clear();
        for (int i = 0; i < 10; i++)
        {
            wayPointsList.Add(new Vector3(Random.Range(0f, 0.4f), 0.005f, Random.Range(0f, 0.4f)));
        }
    }

    // 人物模型移动状态
    enum PeopleMoveStatus
    {
        Move,
        Rotate,
        End
    }
    // 初始化人物模型处于向前移动状态
    PeopleMoveStatus peopleMoveStatus = PeopleMoveStatus.Move;


    Vector3 initDir = Vector3.zero;
    // 计算旋转方向
    private void CalculateRotateData(int index)
    {
        lastQuaternion = person.transform.rotation;
        Vector3 dir = Vector3.zero;
        if (index < wayPointsList.Count - 1)
        {
            // 计算当前轨迹点与下一个轨迹点之间的方向向量
            dir = wayPointsList[index + 1] - wayPointsList[index];
        }
        initDir = dir;
        targetQuaternion = Quaternion.LookRotation(dir); 
    }

    //移动
    private void Move()
    {
        // 向前
        if (peopleMoveStatus == PeopleMoveStatus.Move)
        {
            float nextPointDistance = 0;
            // 没有到最后一个点
            if (wayIndex < wayPointsList.Count - 1)
            {
                nextPointDistance = Vector3.Distance(person.transform.position, wayPointsList[wayIndex + 1]);
                Vector3 direction = (wayPointsList[wayIndex + 1] - wayPointsList[wayIndex]).normalized;
                person.transform.position += Time.deltaTime * direction * MoveSpeed;   // 速度不能太快，否则会无法准确到达指定轨迹点
            }
            // 到达最后一个点
            if (wayIndex == wayPointsList.Count - 1)
            {
                Destroy(person);
            }
            // 即将换到下一个点进行旋转
            if (nextPointDistance <= 0.001f)   // 该值不能太小，实际到达位置与轨迹点间有误差
            {
                peopleMoveStatus = PeopleMoveStatus.Rotate;
                rotateProgress = 0;
                wayIndex++;
                CalculateRotateData(wayIndex);
            }
        }
        // 旋转
        if (peopleMoveStatus == PeopleMoveStatus.Rotate && lastQuaternion != targetQuaternion)
        {
            person.transform.rotation = Quaternion.Lerp(lastQuaternion, targetQuaternion, rotateProgress);
            rotateProgress += Time.deltaTime * RotateSpeed;
            if (lastQuaternion == targetQuaternion || rotateProgress >= 1)
            {
                // 转完了开始移动
                peopleMoveStatus = PeopleMoveStatus.Move;
            }
        }
    }

}
