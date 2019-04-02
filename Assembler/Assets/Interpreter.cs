using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Interpreter : MonoBehaviour
{
    public Text interpreterText;

    GameObject[] memorySlot;
    public GameObject pcSlot, ac1Slot, ac2Slot, ac3Slot;

    Memory[] memories;
    public int pc, ac1, ac2, ac3;

    public List<string> splitedTexts = new List<string>();

    Dictionary<string, int> variables = new Dictionary<string, int>();


    //Dictionary<string, int> variables = new Dictionary<string, int>();



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


        UpdateMemoriesSlotText();
        UpdateRegistersText();
    }




    IEnumerator Decode()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < memories.Length; i++)
        {
            if (pc != i)
                memories[i].transform.parent.GetComponent<Image>().color = Color.white;
        }
        memories[pc].transform.parent.GetComponent<Image>().color = Color.green;

        Memory nextMemory = memories[pc + 1];
        //Debug.Log(nextMemoryData);

        switch (memories[pc].name)
        {
            case "LD":

                pc = Load(nextMemory);

                break;
            case "ADD":

                pc = Add(ref ac1, nextMemory.data);
                break;
            case "SUB":

                pc = Sub(ref ac1, nextMemory.data);
                break;
            case "ST":

                pc = Set(nextMemory.data);
                break;
            case "JMP":

                Jmp(nextMemory.data);
                StartCoroutine(Decode());
                yield break;
            case "HALT":
                Jmp(memories.Length - 1);
                StartCoroutine(Decode());
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



    int Set(int data)
    {
        ac1 = data;
        return pc + 1;
    }
    int Set2(int data)
    {
        ac2 = data;
        return pc + 1;
    }
    int Set3(int data)
    {
        ac3 = data;
        return pc + 1;
    }

    void Print(int ac)
    {
        Debug.Log("Print");
        //if (ac)
        {
            Debug.Log(ac);
        }
        //else
        {
            //Debug.Log("ac is null");
        }

    }

    void Halt()
    {


    }


    void Pos()
    {
        int r = ac1;
        int g = ac2;
        int b = ac3;

    }


    int Add(ref int ac, int value)
    {
        Debug.Log("ADD");
        ac += value;
        return pc + 1;
    }

    int Sub(ref int ac, int value)
    {
        ac -= value;
        return pc + 1;
    }

    void Jz(int ac, int pos)
    {
        // Debug.Log("Jz");
        // if (ac)
        {
            if (ac == 0)
                pc = pos;
        }
        //  else
        {
            //   Debug.Log("ac is null");
        }


    }

    void Jnz(int ac, int pos)
    {
        //Debug.Log("Jnz");
        //if (ac)
        {
            if (ac != 0)
                pc = pos;
        }
        //else
        {
            //   Debug.Log("ac is null");
        }

    }

    void Jmp(int pos)
    {
        pc = pos;
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
        string[] splited = text.Split(' ', ',', '\n'); //separa cada strings do texto e armazena no array

        for (int i = 0; i < splited.Length; i++)
        {
            splited[i] = splited[i].Trim(); //remove os espaços das strings
            if (splited[i] != "")
            {
                splitedTexts.Add(splited[i]); //se a string nao for vazia, adiciona cada elemento do array na lista
            }

        }


        streamReader.Close();
    }

    void StoreInstructionsInMemory()
    {
        int startDataIndex = 0, endDataIndex = 0, startCodeIndex = 0, endCodeIndex = 0;
    
        for (int i = 0; i < splitedTexts.Count; i++)
        {
            if (splitedTexts[i] == ".code")
            {
                startCodeIndex = i + 1;
            }
            else if (splitedTexts[i] == ".endcode")
            {
                endCodeIndex = i;
            }
            else if (splitedTexts[i] == ".data")
            {
                startDataIndex = i + 1;
                Debug.Log(startDataIndex);
            }
            else if (splitedTexts[i] == ".enddata")
            {
                endDataIndex = i;
                Debug.Log(endDataIndex);
            }


        }


        for (int i = 0; i < endCodeIndex - startCodeIndex; i++)
        {
            if (i < memories.Length)
            {
                memories[i].name = splitedTexts[i + startCodeIndex];
                memories[i].UpdateData();
            }
        }

        for (int i = startDataIndex; i < endDataIndex; i += 3)
        {

            string nameText = splitedTexts[i];
            string memoryPosText = splitedTexts[i + 1];
            string dataText = splitedTexts[i + 2];

            int memoryPos = int.Parse(memoryPosText.Substring(1, memoryPosText.Length - 1)); //remove o # da frente da string
            memories[memoryPos].name = nameText;
            memories[memoryPos].data = int.Parse(dataText.Substring(1, dataText.Length - 1)); //remove o # da frente da string

            variables.Add(nameText, memoryPos);

        }
    }
}

