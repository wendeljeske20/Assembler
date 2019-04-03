using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Interpreter : MonoBehaviour
{

    int startDataIndex = 0, endDataIndex = 0, startCodeIndex = 0, endCodeIndex = 0;

    public GameObject variablePanel;
    public GameObject variableSectionPrefab;
    List<GameObject> varSections = new List<GameObject>();

    //public Image cursor;
    public float updateInterval;
    public Text interpreterText;

    GameObject[] memorySlot;
    public GameObject pcSlot, ac1Slot, ac2Slot, ac3Slot;

    Memory[] memories;
    int pc;
    public float ac1, ac2, ac3;

    public List<string> splittedTexts = new List<string>();

   
    Dictionary<string, int> variables = new Dictionary<string, int>();
    List<string> variableNames = new List<string>();




    public Screen screen;

    void Start()
    {

        pc = 0;

        memorySlot = GameObject.FindGameObjectsWithTag("MemorySlot");
        memories = new Memory[memorySlot.Length];
        for (int i = 0; i < memories.Length; i++)
        {
            memories[i] = memorySlot[i].GetComponent<Memory>();
        }

        ReadFile(Application.dataPath + "/text.txt");
        StoreInstructionsInMemory();

        StartCoroutine(Decode());
    }

    private void Update()
    {

        //cursor.GetComponent<RectTransform>().localPosition = new Vector3(0, 200 + pc * 10, 0);
        UpdateMemoriesSlotText();
        UpdateVariablesSlotText();
        UpdateRegistersText();
    }


    

    IEnumerator Decode()
    {
        yield return new WaitForSeconds(updateInterval);

        for (int i = 0; i < memories.Length; i++)
        {
            if (pc != i)
                memories[i].transform.parent.GetComponent<Image>().color = Color.white;
        }
        memories[pc].transform.parent.GetComponent<Image>().color = Color.green;

        Memory firstParameterMemory = memories[pc + 1];
        Memory secondParameterMemory = memories[pc + 2];

        switch (memories[pc].name)
        {
            case "LD":

                pc = Load(firstParameterMemory);

                break;
            case "LD2":

                pc = Load2(firstParameterMemory);

                break;
            case "LD3":

                pc = Load3(firstParameterMemory);

                break;

            case "ST":

                pc = Set(firstParameterMemory);
                break;
            case "ST2":

                pc = Set2(firstParameterMemory);
                break;
            case "ST3":

                pc = Set3(firstParameterMemory);
                break;
            case "ADD":

                pc = Add(firstParameterMemory, secondParameterMemory);
                break;
            case "SUB":

                pc = Sub(firstParameterMemory, secondParameterMemory);
                break;
            case "POS":

                pc = Pos();
                break;
            case "PXL":

                pc = Pxl();
                break;
            case "CLR":

                pc = Clear();
                break;
            case "COS":

                pc = Cos(firstParameterMemory);
                break;
            case "SIN":

                pc = Sin(firstParameterMemory);
                break;
            case "JZ":

                pc = Jz(firstParameterMemory, secondParameterMemory);
                //StartCoroutine(Decode());
                break;
            case "JMP":

                Jmp(firstParameterMemory);
                //StartCoroutine(Decode());
                break;

            case "HALT":
                yield break;
            default:
                Debug.LogError("Command " + memories[pc].name + " not found");
                break;
        }



        pc++;

        if (pc == memories.Length - 1)
        {
            Debug.Log("returned");
            yield break;

        }

        StartCoroutine(Decode());

    }

    int Load(Memory memory)
    {
        if (variables.ContainsKey(memory.name))
        {
            int readPos = variables[memory.name]; //procura a variavel no dicionario e armazena o endereço dela
            ac1 = memories[readPos].data; //recebe o valor da variavel
        }
        else
        {
            Debug.LogError("Key " + memory.name + " not found");
        }
        return pc + 1; // retorna quantas posicões o pc deve pular
    }
    int Load2(Memory memory)
    {
        if (variables.ContainsKey(memory.name))
        {
            int readPos = variables[memory.name]; //procura a variavel no dicionario e armazena o endereço dela
            ac2 = memories[readPos].data; //recebe o valor da variavel
        }
        else
        {
            Debug.LogError("Key " + memory.name + " not found");
        }
        return pc + 1; // retorna quantas posicões o pc deve pular
    }
    int Load3(Memory memory)
    {
        if (variables.ContainsKey(memory.name))
        {
            int readPos = variables[memory.name]; //procura a variavel no dicionario e armazena o endereço dela
            ac3 = memories[readPos].data; //recebe o valor da variavel
        }
        else
        {
            Debug.LogError("Key " + memory.name + " not found");
        }
        return pc + 1; // retorna quantas posicões o pc deve pular
    }


    int Set(Memory memory)
    {
        int readPos = variables[memory.name];
        memories[readPos].data = ac1;

        return pc + 1;
    }
    int Set2(Memory memory)
    {
        int readPos = variables[memory.name];
        memories[readPos].data = ac2;

        return pc + 1;
    }
    int Set3(Memory memory)
    {
        int readPos = variables[memory.name];
        memories[readPos].data = ac3;

        return pc + 1;
    }





    int Add(Memory param1, Memory param2)
    {

        float value = 0;

        if (param2.name == "AC1")
            value = ac1;
        else if (param2.name == "AC2")
            value = ac2;
        else if (param2.name == "AC3")
            value = ac3;
        else
            value = param2.data;

        if (param1.name == "AC1")
            ac1 += value;
        else if (param1.name == "AC2")
            ac2 += value;
        else if (param1.name == "AC3")
            ac3 += value;
        else
            Debug.LogError("It was not possible to add because first parameter needs to receive an Accumulator");


        return pc + 2;
    }

    int Sub(Memory param1, Memory param2)
    {

        float value = 0;

        if (param2.name == "AC1")
            value = ac1;
        else if (param2.name == "AC2")
            value = ac2;
        else if (param2.name == "AC3")
            value = ac3;
        else
            value = param2.data;

        if (param1.name == "AC1")
            ac1 -= value;
        else if (param1.name == "AC2")
            ac2 -= value;
        else if (param1.name == "AC3")
            ac3 -= value;
        else
            Debug.LogError("It was not possible to add because first parameter needs to receive an Accumulator");

        //falta testar se começa com # ou se é um endereço
        return pc + 2;
    }

    int Pos()
    {
        screen.drawingCursorPosition = new Vector2Int((int)ac1, (int)ac2);
        screen.DrawCursor();

        return pc;
    }
    int Pxl()
    {
        screen.DrawPixel((int)ac1, (int)ac2, (int)ac3);
        return pc;
    }

    int Clear()
    {
        screen.ClearScreen();
        return pc;
    }

    int Cos(Memory param1)
    {
        ac1 = Mathf.Cos(param1.data * Mathf.Deg2Rad) * ac2;
        return pc + 1;
    }

    int Sin(Memory param1)
    {
        ac1 = Mathf.Sin(param1.data * Mathf.Deg2Rad) * ac2;
        return pc + 1;
    }

    int GetAccumulatorData(Memory param)
    {
        float data = 0;

        if (param.name == "AC1")
            data = ac1;
        else if (param.name == "AC2")
            data = ac2;
        else if (param.name == "AC3")
            data = ac3;
        else
            Debug.LogError("It was not possible to add because first parameter needs to receive an Accumulator");

        return (int)data;
    }

    int Jz(Memory param1, Memory param2)
    {
        int data = GetAccumulatorData(param1);

        if (data == 0)
        {
            return (int)param2.data - 1;
        }

        return pc + 2;
    }

    int Jnz(Memory param1, Memory param2)
    {
        int data = GetAccumulatorData(param1);

        if (data != 0)
        {
            return (int)param2.data - 1;
        }

        return pc + 2;

    }

    void Jmp(Memory param2)
    {
        pc = (int)param2.data - 1;
        //Debug.Log("jmp " + pos);
        //int result = int.Parse(ac.name.Remove(0, 1)) - int.Parse(value);
        //ac.name = result.ToString();

    }



    void UpdateMemoriesSlotText()
    {
        for (int i = 0; i < memorySlot.Length; i++)
        {
            Text dataText = memorySlot[i].transform.Find("DataText").GetComponent<Text>();
            Text positionText = memorySlot[i].transform.Find("MemoryPositionText").GetComponent<Text>();

            dataText.text = memories[i].name;
            positionText.text = i.ToString();
        }
    }

    void UpdateVariablesSlotText()
    {
        for (int i = 0; i < variableNames.Count; i++)
        {

            
            GameObject varSlot = varSections[i].transform.Find("SlotBackGround").Find("Slot").gameObject;
            varSlot.transform.Find("NameText").GetComponent<Text>().text = variableNames[i];
            varSlot.transform.Find("DataText").GetComponent<Text>().text = memories[variables[variableNames[i]]].data.ToString();
            //varSections[i].

        }
        
        //varSlot.transform.Find("DataText").GetComponent<Text>().text = memories[memoryPos].data.ToString();
    }

    void UpdateRegistersText()
    {

        //pcSlot.transform.Find("DataText").GetComponent<Text>().text = pc.ToString();
        //if (ac1)
        ac1Slot.transform.Find("DataText").GetComponent<Text>().text = ac1.ToString();
        // if (ac2)
        ac2Slot.transform.Find("DataText").GetComponent<Text>().text = ac2.ToString();
        //if (ac3)
        ac3Slot.transform.Find("DataText").GetComponent<Text>().text = ac3.ToString();
    }

    void ReadFile(string filePath)
    {
        StreamReader streamReader = new StreamReader(filePath);
        string text = streamReader.ReadToEnd(); //armazena todo o texto em uma string
        interpreterText.text = text;
        string[] splitted = text.Split(' ', ',', ':', '\n'); //separa cada strings do texto e armazena no array

        for (int i = 0; i < splitted.Length; i++)
        {
            splitted[i] = splitted[i].Trim(); //remove os espaços das strings
            if (splitted[i] != "")
            {
                splittedTexts.Add(splitted[i]); //se a string nao for vazia, adiciona cada elemento do array na lista
            }

        }


        streamReader.Close();
    }
    void StoreInstructionsInMemory()
    {
        

        for (int i = 0; i < splittedTexts.Count; i++)
        {
            switch (splittedTexts[i])
            {
                case ".code":
                    startCodeIndex = i + 1;
                    break;
                case ".endcode":
                    endCodeIndex = i;
                    break;
                case ".data":
                    startDataIndex = i + 1;
                    break;
                case ".enddata":
                    endDataIndex = i;
                    break;
                default:
                    break;
            }



        }


        for (int i = 0; i < endCodeIndex - startCodeIndex; i++)
        {
            if (i < memories.Length)
            {
                memories[i].name = splittedTexts[i + startCodeIndex];
                memories[i].UpdateData();
            }
        }

       

        for (int i = startDataIndex; i < endDataIndex; i += 3)
        {

            string nameText = splittedTexts[i];
            string memoryPosText = splittedTexts[i + 1];
            string dataText = splittedTexts[i + 2];

            int memoryPos = int.Parse(memoryPosText.Substring(1, memoryPosText.Length - 1)); //remove o # da frente da string
            memories[memoryPos].name = nameText;
            memories[memoryPos].data = int.Parse(dataText.Substring(1, dataText.Length - 1)); //remove o # da frente da string

            variables.Add(nameText, memoryPos);
            variableNames.Add(nameText);

            GameObject varSection = Instantiate(variableSectionPrefab, transform.position, Quaternion.identity);
            varSection.transform.SetParent(variablePanel.transform);
            varSection.transform.localScale = Vector3.one;
            //varSection.transform.Find("SlotBackGround").Find("Slot").Find("NameText").GetComponent<Text>().text = nameText;

            

            varSections.Add(varSection);
        }



    }
}

