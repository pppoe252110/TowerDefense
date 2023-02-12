using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDCell : MonoBehaviour
{
    public Material mat;

    public void Init()
    {
        var m = GetComponent<MeshRenderer>();
        m.material = new Material(m.material);
        mat = m.material;
    }
}
