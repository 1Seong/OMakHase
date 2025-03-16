using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.XR;
using static Ingredient;
using static UnityEngine.Rendering.DebugUI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] TextAsset StoryCSV;
    [SerializeField] TextAsset RandomCSV;
    //string csv_FileName;

    [SerializeField] TextMeshProUGUI nameUI;
    [SerializeField] TextMeshProUGUI dialogueUI;
    [SerializeField] RectTransform nextUI;
    [SerializeField] RectTransform skipUI;
    [SerializeField] RectTransform multipleskipUI;


    Dictionary<string, Dialogue> dialogueDic = new Dictionary<string, Dialogue>();
    Dictionary<int, RandomDialogue> randomDialogueDic = new Dictionary<int, RandomDialogue>();

    public static bool isFinish = false;

    [SerializeField]
    private int _Day;
    public int Day { get { return _Day; } }

    [SerializeField]
    private int _Customer;
    public int Customer { get { return _Customer; } }

    [SerializeField]
    private string _Sequence;
    public string Sequence { get { return _Sequence; } }

    [SerializeField]
    private string _Multiple;
    public string Multiple { get { return _Multiple; } }

    [SerializeField] 
    private string _currentID;
    public string currentID { get { return _currentID; } }

    [SerializeField]
    private string _currentDialogue;
    public string currentDialogue {  get { return _currentDialogue; } }

    Dictionary<string, Ingredient.MeatFish> meatFishMap = new Dictionary<string, Ingredient.MeatFish>
    {
        { "�������", Ingredient.MeatFish.pork },
        { "��ġ", Ingredient.MeatFish.tuna },
        { "�߰��", Ingredient.MeatFish.chicken },
        { "����", Ingredient.MeatFish.tuna },
        { "�Ұ��", Ingredient.MeatFish.beef },
        { "����", Ingredient.MeatFish.none }
    };

    Dictionary<string, Ingredient.Vege> vegeMap = new Dictionary<string, Ingredient.Vege>
    {
        { "����", Ingredient.Vege.potato },
        { "�丶��", Ingredient.Vege.tomato },
        { "����", Ingredient.Vege.mushroom },
        { "���", Ingredient.Vege.carrot },
        { "����", Ingredient.Vege.none }
    };

    Dictionary<string, CategoryData> categoryMap;

    Dictionary<string, Ingredient.Main> mainMap = new Dictionary<string, Ingredient.Main>
    {
        { "����", Ingredient.Main.meat },
        { "������", Ingredient.Main.fish },
        { "��ä��", Ingredient.Main.vege },
    };

    Dictionary<string, Ingredient.Cook> cookMap = new Dictionary<string, Ingredient.Cook>
    {
        { "������", Ingredient.Cook.none },
        { "�����丮", Ingredient.Cook.stirFry },
        { "����丮", Ingredient.Cook.roast },
    };

    Dictionary<string, Ingredient.Base> baseMap = new Dictionary<string, Ingredient.Base>
    {
        { "��丮", Ingredient.Base.rice },
        { "���丮", Ingredient.Base.bread },
        { "��丮", Ingredient.Base.noodle },
    };

    Dictionary<string, Personality> personalityMap = new Dictionary<string, Personality>
    {
        { "generous", Personality.Generous},
        { "normal", Personality.Normal },
        { "picky",Personality.Picky },
        { "strict", Personality.Strict },
    };

    // ���� ���� ��� ��� ��?
    private bool isRandom = false;

    // ���� ��� �ε���
    private int indexForRandom = 0;

    // ������ ���� ������ �ִ� �迭
    RandomDialogue[] randomDialogues;

    // ���� ��� �� ����ϰ� ���ƿ� ID
    string backID = "";

    // ���� ��� �� ����ϰ� ���� �ٷ� �ֹ� ��簡 �� ��쿡 ����� �÷���
    bool directOrder = false;

    private void Awake()
    {
        Instance = this;

        categoryMap = new Dictionary<string, CategoryData>
        {
            {"����\r", RecipeManager.instance.getCategoryListDatas()[0].categoryData },
            {"������\r", RecipeManager.instance.getCategoryListDatas()[1].categoryData },
            {"�����ָԹ�\r", RecipeManager.instance.getCategoryListDatas()[2].categoryData },
            {"�ܹ���\r", RecipeManager.instance.getCategoryListDatas()[3].categoryData },
            {"������ġ\r", RecipeManager.instance.getCategoryListDatas()[3].categoryData },
            {"�����\r", RecipeManager.instance.getCategoryListDatas()[4].categoryData },
            {"����\r", RecipeManager.instance.getCategoryListDatas()[4].categoryData },
            {"����\r", RecipeManager.instance.getCategoryListDatas()[5].categoryData },
            {"����\r", RecipeManager.instance.getCategoryListDatas()[6].categoryData },
            {"������\r", RecipeManager.instance.getCategoryListDatas()[7].categoryData },
            {"�����Ľ�Ÿ\r", RecipeManager.instance.getCategoryListDatas()[8].categoryData },
        };

        _Day = 0;
        _Customer = 1;
        _Sequence = string.Format("{0:D2}", 1);

        _currentID = "D" + string.Format("{0:D2}", _Day) + "_C" + string.Format("{0:D2}", _Customer) + "_" + _Sequence + "_";

        DialogueParser theParser = gameObject.GetComponent<DialogueParser>();
        
        Dialogue[] dialogues = theParser.Parse(StoryCSV);

        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogueDic.Add(dialogues[i].dialogueID, dialogues[i]);
        }
        isFinish = true;

        nameUI.text = dialogueDic[currentID].name;
        dialogueUI.text = dialogueDic[currentID].line.Replace('`', ','); ;
        Debug.Log(dialogueDic[currentID].line);



        RandomDialogue[] randomDialogues = theParser.RandomParse(RandomCSV);

        for (int i = 0; i < randomDialogues.Length; i++)
        {
            randomDialogueDic.Add(i, randomDialogues[i]);
        }

    }

    public void GetNextDialogue()
    {

        string[] tmp = _currentID.Split(new char[] { '_' });

        if (dialogueDic.ContainsKey(currentID) && dialogueDic[currentID].nextDialogueID.Contains("EOD"))
        {

            _Day = int.Parse(tmp[0].Substring(1)) + 1;

            // _currentID = "D" + string.Format("{0:D2}", _Day) + "_C" + string.Format("{0:D2}", 1) + "_" + "01" + "_";
            _currentID = dialogueDic[currentID].nextDialogueID.Split(new char[] { '_' })[1].Replace('~', '_');

            if (_currentID.Contains("O"))
            {
                directOrder = true;
            }

            GameManager.instance.nextCustomer();

        }

        else if (dialogueDic.ContainsKey(currentID) && dialogueDic[currentID].nextDialogueID.Contains("EOO"))
        {

            _Customer = int.Parse(tmp[1].Substring(1)) + 1;

            //_currentID = "D" + string.Format("{0:D2}", _Day) + "_C" + string.Format("{0:D2}", _Customer) + "_" + "01" + "_";
            _currentID = dialogueDic[currentID].nextDialogueID.Split(new char[] { '_' })[1].Replace('~', '_');

            if (_currentID.Contains("O"))
            {
                directOrder = true;
            }

            GameManager.instance.nextCustomer();

        }

        else
        {
            if(isRandom == false)
                _currentID = dialogueDic[currentID].nextDialogueID;

            else if (isRandom == true && indexForRandom >= randomDialogues.Length)
            {
                indexForRandom = 0;
                isRandom = false;
                _currentID = backID;

                nextUI.gameObject.SetActive(false);
                skipUI.gameObject.SetActive(true);
            }

            // ���� ��� �����;� �� ��
            if (currentID.Contains("GTR") && isRandom == false)
            {
                isRandom = true;
                int len = int.Parse(_currentID.Split(new char[] { '_' })[1]);
                backID = _currentID.Split(new char[] { '_' })[2].Replace('~', '_');
                randomDialogues = new RandomDialogue[len];
                for (int i = 0; i < len; i++)
                {
                    int randomIndex = UnityEngine.Random.Range(0, randomDialogueDic.Count);
                    Debug.Log(randomDialogueDic[randomIndex].line);
                    randomDialogues[i] = randomDialogueDic[randomIndex];
                }

                Debug.Log("���� ��� ������!");
            }

            if (isRandom == false || directOrder == true)
            {
                directOrder = false;

                // �ֹ� ui Ȱ��ȭ
                if (_currentID.Split(new char[] { '_' })[2] == "O")
                {
                    Debug.Log("!!!!!!!!!!!!!!!");
                    nextUI.gameObject.SetActive(true);

                    string desireMeatfish = dialogueDic[currentID].desireMain.Contains("&&") ? dialogueDic[currentID].desireMain.Split("&&")[0] : "";
                    string desireVege = dialogueDic[currentID].desireMain.Contains("&&") ? dialogueDic[currentID].desireMain.Split("&&")[1] : "";
                    string desireCategory = dialogueDic[currentID].desireCategory;
                    string desirePersonality = dialogueDic[currentID].type;

                    // ����
                    Personality personality = personalityMap.ContainsKey(desirePersonality) ? personalityMap[desirePersonality] : Personality.Generous; //������ �����Ǿ� ���� ������ �⺻������ Generous
                                                                                                                                                        // ī�װ�
                    CategoryData category = categoryMap.ContainsKey(desireCategory) ? categoryMap[desireCategory] : null;

                    // ��� ����
                    Ingredient.MeatFish meatfish = meatFishMap.ContainsKey(desireMeatfish) ? meatFishMap[desireMeatfish] : Ingredient.MeatFish.noCondition;
                    Ingredient.Vege vege = vegeMap.ContainsKey(desireVege) ? vegeMap[desireVege] : Ingredient.Vege.noCondition;
                    Ingredient.Base baseIngred = baseMap.ContainsKey(desireCategory) ? baseMap[desireCategory] : Ingredient.Base.noCondition;
                    Ingredient.Cook cook = cookMap.ContainsKey(desireCategory) ? cookMap[desireCategory] : Ingredient.Cook.noCondition;
                    Ingredient.Main Main = mainMap.ContainsKey(desireMeatfish) ? mainMap[desireMeatfish] : (mainMap.ContainsKey(desireVege) ? mainMap[desireVege] : Ingredient.Main.noCondition);

                    // �ӽ�
                    bool hateMeatFish = false;
                    bool hateVege = false;
                    bool hateBase = false;
                    bool hateCategory = false;

                    // �ӽ�
                    CustomerManager.instance.GetOrder(personality, true, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
                    /*
                    if (meatfish != Ingredient.MeatFish.noCondition )
                    {
                        CustomerManager.instance.GetOrder(personality, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
                    }
                    else
                    {
                        CustomerManager.instance.GetOrder(personality, Main, baseIngred, cook, hateCategory, hateBase);
                    }
                    */

                    skipUI.gameObject.SetActive(false);
                }

                // ���� ���� ui Ȱ��ȭ
                if (currentID.Contains("&&") && !currentID.Contains("R"))
                {
                    string[] chooseTmp = _currentID.Split("&&");

                    multipleskipUI.gameObject.SetActive(true);
                    skipUI.gameObject.SetActive(false);

                    GameObject firstChoose = multipleskipUI.GetChild(0).gameObject.GetComponent<RectTransform>().GetChild(0).gameObject;
                    firstChoose.GetComponent<TextMeshProUGUI>().text = dialogueDic[chooseTmp[0]].line;
                    GameObject secondChoose = multipleskipUI.GetChild(1).gameObject.GetComponent<RectTransform>().GetChild(0).gameObject;
                    secondChoose.GetComponent<TextMeshProUGUI>().text = dialogueDic[chooseTmp[1]].line;
                    return;
                }


                if (currentID.Contains("R"))
                {
                    string temp = currentID;
                    // ���� ���� ó��
                    if (currentID.Contains("&&"))
                    {
                        Debug.Log("��Ƽ");
                        string[] chooseTmp = _currentID.Split("&&");


                        for (int i = 0; i < chooseTmp.Length; i++)
                        {
                            if (CookManager.instance.satisfiedType == CookManager.Result.positive && dialogueDic[chooseTmp[i]].type == "positive")
                            {
                                _currentID = chooseTmp[i];
                                break;
                            }
                            else if (CookManager.instance.satisfiedType == CookManager.Result.neutral && dialogueDic[chooseTmp[i]].type == "neutral")
                            {
                                _currentID = chooseTmp[i];
                                break;
                            }
                            else if (CookManager.instance.satisfiedType == CookManager.Result.negative && (dialogueDic[chooseTmp[i]].type == "negative" || dialogueDic[chooseTmp[i]].type == "s-negative"))
                            {
                                _currentID = chooseTmp[i];
                                break;
                            }
                        }

                        if (temp == _currentID)
                            Debug.LogWarning("�ش�Ǵ� ���� ��簡 �����ϴ�.");

                        nameUI.text = dialogueDic[currentID].name;
                        dialogueUI.text = dialogueDic[currentID].line.Replace('`', ',');
                        return;
                    }
                    // ���� ���� ó��
                    else
                    {
                        Debug.Log("�̱�");
                    }
                }
            }
        }

        if (isRandom == false)
        {
            nameUI.text = dialogueDic[currentID].name;
            dialogueUI.text = dialogueDic[currentID].line.Replace('`', ',');
            Debug.Log(dialogueDic[currentID].line);
        }

        else {
            string desireMeatfish = "";
            string desireVege = "";
            string desireCategory = randomDialogues[indexForRandom].desireCategory;
            string desirePersonality = randomDialogues[indexForRandom].type;

            bool hateMeatFish = false;
            bool hateVege = false;
            bool hateBase = false;
            bool hateCategory = false;

            // ����
            Personality personality = personalityMap.ContainsKey(desirePersonality) ? personalityMap[desirePersonality] : Personality.Generous; //������ �����Ǿ� ���� ������ �⺻������ Generous

            // ��ħ�� ���� ���� ���
            string[] noBatchimMeatFish = { "����", "������", "�������", "��ġ", "�߰��", "����", "�Ұ��" };
            string[] noBatchimVege = { "��ä��", "����", "�丶��", "���" };

            // ��ħ�� �ִ� ���� ���
            string[] withBatchimMeatFish = { };
            string[] withBatchimVege = { "����" };


            // ��ħ�� ���� ���� ���
            string[] noBatchimBase = { "��丮", "���丮", "��丮" };
            string[] noBatchimCook = { "�����丮", "����丮" };
            string[] noBatchimCategory = { "�ܹ���", "������ġ", "�����", "����", "����", "����", "�����Ľ�Ÿ" };

            // ��ħ�� �ִ� ���� ���
            string[] withBatchimBase = { };
            string[] withBatchimCook = { };
            string[] withBatchimCategory = { "����", "������", "�����ָԹ�", "������" };

            switch (randomDialogues[indexForRandom].desireMain)
            {
                case "$$":
                case "!$$":


                    int temp = UnityEngine.Random.Range(0, 1);

                    if (temp == 0)
                    {
                        desireMeatfish = noBatchimMeatFish[UnityEngine.Random.Range(0, noBatchimMeatFish.Length)];

                        if (randomDialogues[indexForRandom].line.Contains("��"))
                        {
                            desireCategory = "��丮";
                        }
                        else if (randomDialogues[indexForRandom].line.Contains("��"))
                        {
                            desireCategory = "���丮";
                        }
                        else if (randomDialogues[indexForRandom].line.Contains("��"))
                        {
                            desireCategory = "��丮";
                        }

                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("$$", desireMeatfish);

                        if (randomDialogues[indexForRandom].desireMain[0] == '!') {
                            hateMeatFish = true;
                        }
                    }
                    else {
                        desireVege = noBatchimVege[UnityEngine.Random.Range(0, noBatchimVege.Length)];

                        if (randomDialogues[indexForRandom].line.Contains("��"))
                        {
                            desireCategory = "��丮";
                        }
                        else if (randomDialogues[indexForRandom].line.Contains("��"))
                        {
                            desireCategory = "���丮";
                        }
                        else if (randomDialogues[indexForRandom].line.Contains("��"))
                        {
                            desireCategory = "��丮";
                        }

                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("$$", desireVege);
                        if (randomDialogues[indexForRandom].desireMain[0] == '!')
                        {
                            hateVege = true;
                        }
                    }
                    
                    break;

                case "%%":
                case "!%%":
                    desireVege = withBatchimVege[UnityEngine.Random.Range(0, withBatchimVege.Length)];

                    if (randomDialogues[indexForRandom].line.Contains("��"))
                    {
                        desireCategory = "��丮";
                    }
                    else if (randomDialogues[indexForRandom].line.Contains("��"))
                    {
                        desireCategory = "���丮";
                    }
                    else if (randomDialogues[indexForRandom].line.Contains("��"))
                    {
                        desireCategory = "��丮";
                    }

                    randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("%%", desireVege);
                    if (randomDialogues[indexForRandom].desireMain[0] == '!')
                    {
                        hateVege = true;
                    }
                    break;

                default:
                    break;
            }


            switch (randomDialogues[indexForRandom].desireCategory)
            {
                case "##":
                case "!##":


                    int temp = UnityEngine.Random.Range(0, 3);

                    if (temp == 0)
                    {
                        desireCategory =  noBatchimBase[UnityEngine.Random.Range(0, noBatchimBase.Length)];

                        if (randomDialogues[indexForRandom].desireCategory[0] == '!')
                        {
                            hateBase = true;
                        }
                    }

                    else if(temp == 1) {
                        desireCategory = noBatchimCook[UnityEngine.Random.Range(0, noBatchimCook.Length)];
                    }

                    else
                    {
                        desireCategory = noBatchimCategory[UnityEngine.Random.Range(0, noBatchimCategory.Length)];
                        if (randomDialogues[indexForRandom].desireCategory[0] == '!')
                        {
                            hateCategory = true;
                        }
                    }

                    randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("##", desireCategory);
                    break;

                case "@@":
                case "!@@":
                    desireCategory = withBatchimCategory[UnityEngine.Random.Range(0, withBatchimCategory.Length)];
                    randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("@@", desireCategory);
                    break;

                default:
                    break;
            }

            if (randomDialogues[indexForRandom].line.Contains("**")) 
            {

                int temp = UnityEngine.Random.Range(0, 7);

                switch (temp)
                {
                    case 0:
                        desireMeatfish = noBatchimMeatFish[UnityEngine.Random.Range(0, noBatchimMeatFish.Length)];
                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("**", desireMeatfish);
                        break;

                    case 1:
                        desireVege = noBatchimVege[UnityEngine.Random.Range(0, noBatchimVege.Length)];
                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("**", desireVege);
                        break;

                    case 2:
                        desireVege = withBatchimVege[UnityEngine.Random.Range(0, withBatchimVege.Length)];
                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("**", desireVege);
                        break;

                    case 3:
                        desireCategory = noBatchimBase[UnityEngine.Random.Range(0, noBatchimBase.Length)];
                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("**", desireCategory);
                        break;

                    case 4:
                        desireCategory = noBatchimCook[UnityEngine.Random.Range(0, noBatchimCook.Length)];
                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("**", desireCategory);
                        break;

                    case 5:
                        desireCategory = noBatchimCategory[UnityEngine.Random.Range(0, noBatchimCategory.Length)];
                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("**", desireCategory);
                        break;

                    case 6:
                        desireCategory = withBatchimCategory[UnityEngine.Random.Range(0, withBatchimCategory.Length)];
                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("**", desireCategory);
                        break;

                    default:
                        Debug.Log("Invalid category selected.");
                        break;
                }


            }

            CategoryData category = categoryMap.ContainsKey(desireCategory + "\r") ? categoryMap[desireCategory + "\r"] : null;
            Debug.Log(category);

            // ��� ����
            Ingredient.MeatFish meatfish = meatFishMap.ContainsKey(desireMeatfish) ? meatFishMap[desireMeatfish] : Ingredient.MeatFish.noCondition;
            Ingredient.Vege vege = vegeMap.ContainsKey(desireVege) ? vegeMap[desireVege] : Ingredient.Vege.noCondition;
            Ingredient.Base baseIngred = baseMap.ContainsKey(desireCategory) ? baseMap[desireCategory] : Ingredient.Base.noCondition;
            Ingredient.Cook cook = cookMap.ContainsKey(desireCategory) ? cookMap[desireCategory] : Ingredient.Cook.noCondition;
            Ingredient.Main Main = mainMap.ContainsKey(desireMeatfish) ? mainMap[desireMeatfish] : (mainMap.ContainsKey(desireVege) ? mainMap[desireVege] : Ingredient.Main.noCondition);


            if (meatfish != Ingredient.MeatFish.noCondition || vege != Ingredient.Vege.noCondition)
            {
                if (baseIngred != Ingredient.Base.noCondition || cook != Ingredient.Cook.noCondition)
                {
                    Debug.Log(1);
                    CustomerManager.instance.GetOrder(personality, false, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
                }

                else if (category != null)
                {
                    Debug.Log(2);
                    CustomerManager.instance.GetOrder(personality, false, meatfish, vege, category, hateMeatFish, hateVege, hateBase);
                }

                else {
                    Debug.Log(3);
                    CustomerManager.instance.GetOrder(personality, false, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
                }
            }

            else if (Main != Ingredient.Main.noCondition)
            {
                if (baseIngred != Ingredient.Base.noCondition || cook != Ingredient.Cook.noCondition)
                {
                    Debug.Log(4);
                    CustomerManager.instance.GetOrder(personality, false, Main, baseIngred, cook, hateCategory, hateBase);
                }
                else if (category != null)
                {
                    Debug.Log(5);
                    CustomerManager.instance.GetOrder(personality, false, Main, category, hateCategory, hateBase);
                }

                else {
                    Debug.Log(6);
                    CustomerManager.instance.GetOrder(personality, false, Main, baseIngred, cook, hateCategory, hateBase);
                }
            }

            else
            {
                Debug.Log("����");
                CustomerManager.instance.GetOrder(personality, false, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
            }

            nameUI.text = "�մ�";
            dialogueUI.text = randomDialogues[indexForRandom].line.Replace('`', ',');
            nextUI.gameObject.SetActive(true);
            skipUI.gameObject.SetActive(false);

            indexForRandom++;
            _Customer++;

        }

    }

    public void GetNextDialogueMultiple(string index)
    {
        string[] chooseTmp = _currentID.Split("&&");

        if (index == "a")
            _currentID = chooseTmp[0];
        if (index == "b")
            _currentID = chooseTmp[1];

        multipleskipUI.gameObject.SetActive(false);
        skipUI.gameObject.SetActive(true);
    }
}
