using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    public string name = "";
    public float data = 0;

    public void UpdateData()
    {
        if (name.Length > 0)
        {
            float.TryParse(name, out data);
            if (name.Substring(0, 1) == "#")
            {
                data = int.Parse(name.Substring(1, name.Length - 1));
            }

            

            

        }
    }

}
