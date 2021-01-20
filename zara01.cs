using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;
using DrawData;  // 导入自定义命名空间

public class zara01 : MonoBehaviour
{
    private string PersonPath = "Prefabs/SkelMesh_Bodyguard_03";    // 人物模型地址
    private Vector3 PersonScale = new Vector3(0.06f, 0.06f, 0.06f); // 人物尺寸
    private int FrameCount = 0;       // 帧数
    private int PersonCount = 0;      // 累计出现的人物数量
    private int Count;                // 数据集列数
    private float MoveMax = 0.8f;     // 每次移动的最大距离
    private bool IsRepeat = false;    // 人物再次出现则该值为 true
    private int stopFrameCount = 5;   // 停止运动的时间，超过该值则删除对应人物
    DataTable dt;                     // 创建DataTable以存储数据集
    public struct CreatePerson
    {
        public GameObject person;   // 创建的人物模型
        public int index;           // 人物模型序号
        public int StopFrames;      // 停止运动的时间
    };
    CreatePerson[] NewPerson = new CreatePerson[150]; // 画面中累计出现148个人

    // Start is called before the first frame update
    void Start()
    {
        // 1.获取数据集
        dt = FilePath.OpenCSV("F:/大四/华科-轨迹预测/data/ucy/zara/zara01/pixel_pos.csv");
        // 2. 筛选：取出第0帧数据
        DataTable dtNew = new DataTable();
        dtNew = dt.Copy();
        Count = dt.Columns.Count;
        for (int i = Count - 1; i >= 0; i--)
        {
            if (Convert.ToInt32(dtNew.Rows[0][i].ToString()) != 0)
            {
                dtNew.Columns.RemoveAt(i);
            }
        }

        // 3. 根据取出的数据开始创建人物模型
        for (int i = 0; i < dtNew.Columns.Count; i++)
        {
            NewPerson[PersonCount].person = (GameObject)Instantiate(Resources.Load(PersonPath));  // 创建人物模型
            NewPerson[PersonCount].person.transform.localScale = PersonScale;                     // 修正人物尺寸
            NewPerson[PersonCount].person.transform.position = new Vector3(float.Parse(dtNew.Rows[2][i].ToString()), 0.005f, float.Parse(dtNew.Rows[3][i].ToString()));   // 改变人物位置（x, 0.005, z）
            NewPerson[PersonCount].index = Convert.ToInt32(dtNew.Rows[1][i].ToString());          // 记录人物序号
            NewPerson[PersonCount].StopFrames = 0;
            PersonCount++;  // 增加人物数量
        }
    }


    // Update is called once per frame
    void Update()
    {
        FrameCount++;

        // 在数据集所确定的帧数范围内执行下述操作，否则结束
        if (FrameCount <= Convert.ToInt32(dt.Rows[0][Count-1].ToString()))
        {
            //取出当前帧数据
            DataTable dtNew = new DataTable();
            dtNew = dt.Copy();
            for (int i = Count - 1; i >= 0; i--)
            {
                if (Convert.ToInt32(dtNew.Rows[0][i].ToString()) != FrameCount)
                {
                    dtNew.Columns.RemoveAt(i);
                }
            }

            // 如果当前帧有数据则执行下一步操作，否则跳过当前帧
            if (dtNew.Columns.Count > 0)
            {
                // 每个人物模型StopFrames加1，如果当前帧再次出现则归0
                for (int j = 0; j < PersonCount; j++)
                {
                    NewPerson[j].StopFrames++;
                    // 停止运动时间超过设定值，删除该人物及其对应的结构体数组元素
                    if (NewPerson[j].StopFrames > stopFrameCount)
                    {
                        Destroy(NewPerson[j].person);
                        Array.Clear(NewPerson, j, 1);
                    }
                }

                // 根据人物序号是否一致决定是在已建人物上操作还是新建人物上操作
                for (int m = 0; m < dtNew.Columns.Count; m++)
                {
                    // 获取下一个出现的点的位置
                    Vector3 nextPointPosition = new Vector3(float.Parse(dtNew.Rows[2][m].ToString()), 0.005f, float.Parse(dtNew.Rows[3][m].ToString()));
                    for (int n = 0; n < PersonCount; n++)
                    {
                        // 排除被删除掉的人物
                        if (NewPerson[n].index > 0)
                        {
                            IsRepeat = false;
                            // 如果与之前出现的是同一个人则继续移动到下一个点，否则新建人物
                            if (Convert.ToInt32(dtNew.Rows[1][m].ToString()) == NewPerson[n].index)
                            {
                                // 与下一个点间的距离
                                float nextPointDistance = Vector3.Distance(NewPerson[n].person.transform.position, nextPointPosition);
                                // 旋转
                                Vector3 dir = (nextPointPosition - NewPerson[n].person.transform.position).normalized;
                                NewPerson[n].person.transform.rotation = Quaternion.LookRotation(dir);
                                // 移动到下一个点
                                NewPerson[n].person.transform.position = Vector3.MoveTowards(NewPerson[n].person.transform.position, nextPointPosition, MoveMax * Time.deltaTime);

                                IsRepeat = true;                // 人物再次出现
                                NewPerson[n].StopFrames = 0;    // 人物再次出现则该值归0
                                break;
                            }
                        }
                    }
                    // 出现新人物，新建
                    if (IsRepeat == false)
                    {
                        NewPerson[PersonCount].person = (GameObject)Instantiate(Resources.Load(PersonPath));  // 创建人物模型
                        NewPerson[PersonCount].person.transform.localScale = PersonScale;                     // 修正人物尺寸
                        NewPerson[PersonCount].person.transform.position = nextPointPosition;                 // 改变人物位置（x, 0.005, z）
                        NewPerson[PersonCount].index = Convert.ToInt32(dtNew.Rows[1][m].ToString());          // 记录人物序号
                        NewPerson[PersonCount].StopFrames = 0;  //  第一次出现，该值初始化为0
                        PersonCount++;                          // 累计出现的人物数量+1
                    }
                }
            }
        }
    }
}
