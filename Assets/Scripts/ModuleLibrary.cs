using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]

[CreateAssetMenu(menuName ="ScriptableObject/ModuleLibrary")]
public class ModuleLibrary : ScriptableObject
{

    [SerializeField]
    private GameObject impotedModules;

    public static Dictionary<string, List<Module>> moduleLibrary = new Dictionary<string, List<Module>>();


    public void ImportedModule()
    {
        foreach (Transform child in impotedModules.transform)
        {
            Mesh mesh = child.GetComponent<MeshFilter>().sharedMesh;
            string name = child.name.Replace(" ", "");
            if(name=="10001100")
            {
                Debug.Log("Check");
            }
            AddModuleTolibrary(name,mesh);
        }

        AddModuleTolibrary("11111111", null);
        AddModuleTolibrary("00000000", null);
        //Debug.Log("Moudle数：" + moduleLibrary.Count);

        //for (int i = 0; i < 256; i++)
        //{
        //    string name = "00000000";

        //    string index = Convert.ToString(i, 2);

        //    string newName = name.Substring(0, 8 - index.Length) + index;

        //    if (!moduleLibrary.ContainsKey(newName))
        //    {
        //        Debug.Log("没有" + newName + "模块");
        //    }
        //}
    }

    public void AddModuleTolibrary(string name,Mesh mesh)
    {
        if (!moduleLibrary.ContainsKey(name))
        {
            moduleLibrary.Add(name, new List<Module>());
            if (!RotationEqualCheck(name))
            {
                string newName = RotateName(name, 1);
                if(!moduleLibrary.ContainsKey(newName))
                {
                    moduleLibrary.Add(newName, new List<Module>());
                    moduleLibrary[newName].Add(new Module(newName, mesh, 1, false));
                }
            }
            if(!RatationTwiceEqualCheck(name))
            {
                string newName = RotateName(name, 2);
                if (!moduleLibrary.ContainsKey(newName))
                {
                    moduleLibrary.Add(newName, new List<Module>());
                    moduleLibrary[newName].Add(new Module(newName, mesh, 2, false));
                }

                string newName2 = RotateName(name, 3);
                if (!moduleLibrary.ContainsKey(newName2))
                {
                    moduleLibrary.Add(newName2, new List<Module>());
                    moduleLibrary[newName2].Add(new Module(newName2, mesh, 3, false));
                }
            }

            //翻转部分
            if(!FlipRotationEqualCheck(name))
            {
                string newName = FlipName(name);

                if (!moduleLibrary.ContainsKey(newName))
                {
                    moduleLibrary.Add(newName, new List<Module>());
                    moduleLibrary[newName].Add(new Module(newName, mesh, 0, true));
                }

                if (!RotationEqualCheck(newName))
                {
                    string newFlipName = RotateName(newName, 1);
                    if (!moduleLibrary.ContainsKey(newFlipName))
                    {
                        moduleLibrary.Add(newFlipName, new List<Module>());
                        moduleLibrary[newFlipName].Add(new Module(newFlipName, mesh, 1, true));
                    }
                }
                if (!RatationTwiceEqualCheck(name))
                {
                    string newFlipName = RotateName(newName, 2);
                    if (!moduleLibrary.ContainsKey(newFlipName))
                    {
                        moduleLibrary.Add(newFlipName, new List<Module>());
                        moduleLibrary[newFlipName].Add(new Module(newFlipName, mesh, 2, true));
                    }

                    string newFlipName2 = RotateName(newName, 3);
                    if (!moduleLibrary.ContainsKey(newFlipName2))
                    {
                        moduleLibrary.Add(newFlipName2, new List<Module>());
                        moduleLibrary[newFlipName2].Add(new Module(newFlipName2, mesh, 3, true));
                    }
                }
            }


            moduleLibrary[name].Add(new Module(name, mesh, 0, false));        
        }
    }


    public static List<Module> GetMoudlByBit(string bit)
    {
        List<Module> moduleList = new List<Module>();
        moduleLibrary.TryGetValue(bit, out moduleList);
        return moduleList;
    }

    public string RotateName(string name,int time)
    {
        string result = name;

        for (int i = 0; i < time; i++)
        {
            result = result[3] + result.Substring(0, 3) + result[7] + result.Substring(4, 3);
        }
        return result;
    }

    public string FlipName(string name)
    {
        return name[3].ToString() + name[2] + name[1] + name[0] + name[7] + name[6] + name[5] + name[4];
    }

    public bool RotationEqualCheck(string name)
    {
        return name[0] == name[1] && name[1] == name[2] && name[2] == name[3] && name[3] == name[4] && name[4] == name[5] && name[5] == name[6] && name[6] == name[7];        
    }

    public bool RatationTwiceEqualCheck(string name)
    {
        return name[0] == name[2] && name[1] == name[3] && name[4] == name[6] && name[5] == name[7];
    }

    public bool FlipRotationEqualCheck(string name)
    {
        string symetry_vertical = name[3].ToString() + name[2] + name[1] + name[0] + name[7] + name[6] + name[5] + name[4];

        string symetry_horizontal = name[1].ToString() + name[0] + name[3] + name[2] + name[5] + name[4] + name[7] + name[6];

        string symetry_02 = name[0].ToString() + name[3] + name[2] + name[1] + name[4] + name[7] + name[6] + name[5];

        string symetry_03 = name[2].ToString() + name[1] + name[0] + name[3] + name[6] + name[5] + name[4] + name[7];

        return name == symetry_vertical || name == symetry_horizontal || name == symetry_02 || name == symetry_03;
    }
}
