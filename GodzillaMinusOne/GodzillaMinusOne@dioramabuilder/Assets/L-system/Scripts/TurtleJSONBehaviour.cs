using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEngine;

public class TurtleBehaviour : MonoBehaviour
{
    LindenmayerClass _lindenmayer = null;


    // Start is called before the first frame update
    void Start()
    {
        //WriteToJSON();
        ReadFromJSON(@"Assets\Json\lindenmayer");
        //Production(1,lindenmayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WriteToJSON(LindenmayerClass lindenmayer = null)
    {
        if (lindenmayer == null)
        {
            lindenmayer = new LindenmayerClass()
            {
                generation = 3,
                heading = 31,
                axiom = "SPX",
                rule0 = "X>/P[-FX]+FX",
                rule1 = "P>FF",
                rule2 = "S>FF"
            };
        }

        if (!File.Exists(@"Assets\Json\lindenmayer" + lindenmayer.axiom + ".json"))
        {
            Debug.Log("Writing lindenmayer: " + lindenmayer.axiom);
            string json = JsonUtility.ToJson(lindenmayer);
            File.WriteAllText(@"Assets\Json\lindenmayer" + lindenmayer.axiom + ".json", json); 
        }
    }
    private void ReadFromJSON(string path)
    {
        if (File.Exists(path))
        {
            Debug.Log("reading lindenmayer: " + path);
            string json = JsonUtility.ToJson(File.ReadAllText(path));
            _lindenmayer = JsonUtility.FromJson<LindenmayerClass>(json);
        }

    }


    void Production(int _generations, LindenmayerClass lindenmayer)
    {
        string sLindenmayer = "";

        if (sLindenmayer.Length < 1)
        {
            sLindenmayer = lindenmayer.axiom;
        }

        for (int i = 0; i < _generations; i++)
        {
            StringBuilder newLindenmayer = new StringBuilder();

            Debug.Log(newLindenmayer.MaxCapacity);

            string[] rule0 = lindenmayer.rule0.Split('>');
            string[] rule1 = lindenmayer.rule1.Split('>');
            string[] rule2 = lindenmayer.rule2.Split('>');

            for (int j = 0; j < sLindenmayer.Length; j++)
            {
                if (sLindenmayer.Substring(j, rule0[0].Length) == rule0[0])
                {
                    newLindenmayer.Append(rule0[1]);
                }
                else if (sLindenmayer.Substring(j, rule1[0].Length) == rule1[0])
                {
                    newLindenmayer.Append(rule1[1]);
                }
                else if (sLindenmayer.Substring(j, rule1[0].Length) == rule2[0])
                {
                    newLindenmayer.Append(rule2[1]);
                }
                else
                {
                    newLindenmayer.Append(sLindenmayer.Substring(j, rule1.Length));
                }

            }

            sLindenmayer = newLindenmayer.ToString();
        }
    }

    void Move(float _distance)
    {
        this.transform.localPosition += (transform.forward * _distance);
    }

    void Draw(float _distance)
    {
        GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        line.transform.position = this.transform.position + (transform.forward * _distance / 2f);
        line.transform.rotation = this.transform.rotation;
        line.transform.rotation *= Quaternion.AngleAxis(90f, Vector3.right);
        line.transform.localScale = new Vector3(_distance / 2f, _distance / 2f, _distance / 2f);
        line.transform.parent = this.transform;

        Move(_distance);

        GameObject cap = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        cap.transform.position = this.transform.position;
        cap.transform.localScale = new Vector3(_distance / 2f, _distance / 2f, _distance / 2f);
        cap.transform.parent = this.transform;

    }

    void Turn(float _heading)
    {
        this.transform.rotation *= Quaternion.AngleAxis(_heading, Vector3.up);
    }
    void Pitch(float _heading)
    {
        this.transform.rotation *= Quaternion.AngleAxis(_heading, Vector3.left);
    }
    void Roll(float _heading)
    {
        this.transform.rotation *= Quaternion.AngleAxis(_heading, Vector3.forward);
    }
}
