using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class TurtleLogic : MonoBehaviour
{




    [SerializeField]
    private float distance;
    [SerializeField]
    private float heading;
    [SerializeField]
    private int generations;
    [SerializeField]
    private string axiom;
    [SerializeField]
    private string rulep0;
    [SerializeField]
    private string rulep1;
    [SerializeField]
    private string rulep2;
    [SerializeField]
    private string rulep3;
    [SerializeField]
    private string rulep4;
    [SerializeField]
    private bool onStart;
    [SerializeField]
    private bool resume;
    [SerializeField]
    private Material lineMaterial;
    [SerializeField]
    private Material capMaterial;


    private string lindenmayer;
    private GameObject drawParent;
    private Stack savedPosition = new Stack();
    private Stack savedRotation = new Stack();
    int iteration = 0;

    // Start is called before the first frame update
    void Start()
    {
        lindenmayer = axiom;

        drawParent = new GameObject();

        Production(generations);

        axiom = lindenmayer;
        //Debug.Log(lindenmayer);

        if (onStart)
        {
            //ReadFullLindenmayer();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (resume)
        {
            ReadLindenmayer();
        }
    }

    void Production(int _generations)
    {
        if (lindenmayer == null)
        {
            lindenmayer = axiom;
        }

        lindenmayer = ConvertNotation(lindenmayer);
        rulep0 = ConvertNotation(rulep0);
        rulep1 = ConvertNotation(rulep1);

        for (int i = 0; i < _generations; i++)
        {
            StringBuilder newLindenmayer = new StringBuilder();

            string[] rule0 = rulep0.Split('>');
            string[] rule1 = rulep1.Split('>');
            string[] rule2 = rulep2.Split('>');

            for (int j = 0; j < lindenmayer.Length; j++)
            {
                if (lindenmayer.Substring(j,1) == rule0[0])
                {
                    newLindenmayer.Append(rule0[1]);
                }else if (lindenmayer.Substring(j,1) == rule1[0])
                {
                    newLindenmayer.Append(rule1[1]);
                }
                else if (lindenmayer.Substring(j,1) == rule2[0])
                {
                    newLindenmayer.Append(rule2[1]);
                }
                else
                {
                    newLindenmayer.Append(lindenmayer.Substring(j,1));
                }

            }

            lindenmayer = newLindenmayer.ToString();

            //newLindenmayer.Replace(rule0[0], rule0[1]);

        }
    }

    public void Mandelbrot(int _generation, float c)
    {
        float z = 0f;

        for (int i = 0; i < _generation; i++)
        {
            z = Mathf.Pow(z, _generation) + c;
        }
    }

    private string ConvertNotation(string _lindenmayer)
    {
        _lindenmayer = _lindenmayer.Replace("Fr", "G");
        _lindenmayer = _lindenmayer.Replace("Fl", "F");
        _lindenmayer = _lindenmayer.Replace("\\","\"");
        _lindenmayer = _lindenmayer.Replace("∧", "^");
        return _lindenmayer;
    }

    void ReadFullLindenmayer()
    {
        foreach (char letter in lindenmayer)
        {
            if (char.IsUpper(letter))
            {
                Draw(distance);
            }
            else if (letter == 'f') // forward
            {
                Move(distance);
            }
            else if (letter == 'b') // back
            {
                Move(-distance);
            }
            else if (letter == 'r') // strafeRight
            {
                //right90;
                //Move(Distance);
                //left90;
            }
            else if (letter == 'l') // strafeLeft
            {
                //left90;
                //Move(Distance);
                //right90;
            }
            else if (letter == 'u') // penUp
            {
                //penstate.false;
            }
            else if (letter == 'p') // penDown
            {
                //penstate.true;
            }
            else if (letter == 'c') // ShiftColor
            {
                //Shiftcolor();
            }
            else if (letter == 'h') // Hide
            {
                //visibility.false;
            }
            else if (letter == '+')
            {
                Turn(-heading);
            }
            else if (letter == '-')
            {
                Turn(+heading);
            }
            else if (letter == '&')
            {
                Pitch(+heading);
            }
            else if (letter == '%')
            {
                Pitch(-heading);
            }
            else if (letter == '@')
            {
                Roll(+heading);
            }
            else if (letter == '#')
            {
                Roll(-heading);
            }
            else if (letter == '|')
            {
                Turn(heading * 2);
            }
            else if (letter == '[')
            {
                savedPosition.Push(this.transform.position);
                savedRotation.Push(this.transform.rotation);
            }
            else if (letter == ']')
            {
                this.transform.position = (Vector3)savedPosition.Pop();
                this.transform.rotation = (Quaternion)savedRotation.Pop();
            }
        }
    }


    /*
     * float alpha
     * Int Generation 
     * String Axiom
     * String Rule0 
     * String Rule1 
     * String Rule2 
     * String RuleZ 
     * 
     * */
    


    void ReadLindenmayer()
    {
        if (iteration < lindenmayer.Length)
        {
            string _readString = lindenmayer.Substring(iteration, 1);

            if (char.IsUpper(_readString[0]))
            {
                Draw(distance);
            }
            else if (_readString == "f")
            {
                Move(distance);
            }
            else if (_readString == "+")
            {
                Turn(-heading);
            }
            else if (_readString == "-")
            {
                Turn(+heading);
            }
            else if (_readString == "&")
            {
                Pitch(+heading);
            }
            else if (_readString == "^")
            {
                Pitch(-heading);
            }
            else if (_readString == "\"")
            {
                Roll(+heading);
            }
            else if (_readString == "/")
            {
                Roll(-heading);
            }
            else if (_readString == "|")
            {
                Turn(heading*2);
            }
            else if (_readString == "[")
            {
                savedPosition.Push(this.transform.position);
                savedRotation.Push(this.transform.rotation);
            }
            else if (_readString == "]")
            {
                this.transform.position = (Vector3)savedPosition.Pop();
                this.transform.rotation = (Quaternion)savedRotation.Pop();
            }
        }
        iteration++;
    }

    void Move(float _distance)
    {
        this.transform.localPosition += (transform.forward * _distance);
    }

    void Draw(float _distance)
    {
        GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        line.transform.position = this.transform.position +(transform.forward * _distance/2f);
        line.transform.rotation = this.transform.rotation;
        line.transform.rotation *= Quaternion.AngleAxis(90f, Vector3.right);
        line.transform.localScale = new Vector3(_distance/2f, _distance/2f, _distance/2f);
        line.transform.parent = drawParent.transform;
        
        var rend = line.GetComponent<Renderer>();
        rend.material = lineMaterial;

        Move(_distance);

        GameObject cap = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        cap.transform.position = this.transform.position;
        cap.transform.localScale = new Vector3(_distance/2f, _distance/2f, _distance/2f);
        cap.transform.parent = drawParent.transform;

        var rend2 = cap.GetComponent<Renderer>();
        rend2.material = capMaterial;
    }

    void Turn(float _heading)
    {
        this.transform.rotation *= Quaternion.AngleAxis(_heading,Vector3.up);
    }
    void Pitch(float _heading)
    {
        this.transform.rotation *= Quaternion.AngleAxis(_heading,Vector3.left);
    }
    void Roll(float _heading)
    {
        this.transform.rotation *= Quaternion.AngleAxis(_heading,Vector3.forward);
    }

}
