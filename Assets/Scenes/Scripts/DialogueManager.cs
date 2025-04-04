using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private string _pastID;
    public string pastID { get { return _pastID; } }

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
        { "����", Ingredient.MeatFish.salmon },
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
    [SerializeField]
    private bool _isRandom = false;
    public bool IsRandom { get => _isRandom; }

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
        //Debug.Log(dialogueDic[currentID].line);



        RandomDialogue[] randomDialogues = theParser.RandomParse(RandomCSV);

        for (int i = 0; i < randomDialogues.Length; i++)
        {
            randomDialogueDic.Add(i, randomDialogues[i]);
        }

    }

    public void GetNextDialogue()
    {
        _pastID = _currentID;
        string[] tmp = _currentID.Split(new char[] { '_' });

        //else
        {
            if (_isRandom == false)
            {
                if (dialogueDic.ContainsKey(currentID) && dialogueDic[currentID].nextDialogueID.Contains("EOD"))
                {

                    //_Day = int.Parse(tmp[0].Substring(1)) + 1;

                    // _currentID = "D" + string.Format("{0:D2}", _Day) + "_C" + string.Format("{0:D2}", 1) + "_" + "01" + "_";
                    _currentID = dialogueDic[currentID].nextDialogueID.Split(new char[] { '_' })[1].Replace('~', '_');

                    if (_currentID.Contains("O"))
                    {
                        directOrder = true;
                    }

                    if (_pastID.Split(new char[] { '_' })[1] != "C00")
                        GameManager.instance.nextCustomer();
                }

                else if (dialogueDic.ContainsKey(currentID) && dialogueDic[currentID].nextDialogueID.Contains("EOO"))
                {

                    // _Customer = int.Parse(tmp[1].Substring(1)) + 1;

                    //_currentID = "D" + string.Format("{0:D2}", _Day) + "_C" + string.Format("{0:D2}", _Customer) + "_" + "01" + "_";
                    _currentID = dialogueDic[currentID].nextDialogueID.Split(new char[] { '_' })[1].Replace('~', '_');

                    if (_currentID.Contains("O"))
                    {
                        directOrder = true;
                    }

                    if (_pastID.Split(new char[] { '_' })[1] != "C00")
                        GameManager.instance.nextCustomer();
                }
                else
                    _currentID = dialogueDic[currentID].nextDialogueID;
            }

            else if (_isRandom == true && indexForRandom >= randomDialogues.Length)
            {
                indexForRandom = 0;
                _isRandom = false;
                _currentID = backID;


                if (_pastID.Split(new char[] { '_' })[1] != "C00")
                    GameManager.instance.nextCustomer();

                if (_currentID.Contains("O"))
                {
                    directOrder = true;
                }
                else {
                    nextUI.gameObject.SetActive(false);
                    skipUI.gameObject.SetActive(true);
                }

            }

            // ���� ��� �����;� �� ��
            if (currentID.Contains("GTR") && _isRandom == false)
            {
                _isRandom = true;
                backID = _currentID.Split(new char[] { '_' })[1].Replace('~', '_');

                int len;

                string tmp1 = pastID.Split(new char[] { '_' })[1];
                string tmp2 = backID.Split(new char[] { '_' })[1];
                if (int.Parse(tmp1.Substring(1, tmp1.Length - 1)) < int.Parse(tmp2.Substring(1, tmp2.Length - 1)))
                {
                    len = int.Parse(tmp2.Substring(1, tmp2.Length - 1)) - int.Parse(tmp1.Substring(1, tmp1.Length - 1)) - 1;
                }
                else
                    len = GameManager.instance.GetCustomerNum(GameManager.instance.day)-GameManager.instance.customerNum;
                Debug.Log(len + "�� ���� ������");


                randomDialogues = new RandomDialogue[len];
                for (int i = 0; i < len; i++)
                {
                    int randomIndex = UnityEngine.Random.Range(0, randomDialogueDic.Count);
                    Debug.Log(randomDialogueDic[randomIndex].line + " | " + System.Guid.NewGuid());
                    randomDialogues[i] = randomDialogueDic[randomIndex];
                }


                if (_currentID.Contains("O"))
                {
                    directOrder = true;
                    //nextUI.gameObject.SetActive(true);
                    //skipUI.gameObject.SetActive(false);
                }
                Debug.Log(directOrder);
                Debug.Log("���� ��� ������!");
            }


            if ((_isRandom == false || directOrder == true) && !(_currentID.Contains("GTR")))
            {


                directOrder = false;
                Debug.Log(_currentID);
                // �ֹ� ui Ȱ��ȭ
                if (_currentID.Split(new char[] { '_' })[2] == "O")
                {
                    nextUI.gameObject.SetActive(true);

                    string desireMeatfish = dialogueDic[currentID].desireMain.Contains("&&") ? dialogueDic[currentID].desireMain.Split("&&")[0] : "";
                    string desireVege = dialogueDic[currentID].desireMain.Contains("&&") ? dialogueDic[currentID].desireMain.Split("&&")[1] : "";
                    string desireCategory = dialogueDic[currentID].desireCategory;
                    string desirePersonality = dialogueDic[currentID].type;

                    // ����
                    Personality personality = personalityMap.ContainsKey(desirePersonality) ? personalityMap[desirePersonality] : Personality.Generous; //������ �����Ǿ� ���� ������ �⺻������ Generous
                                                                                                                                                        // ī�װ�
                    CategoryData category = categoryMap.ContainsKey(desireCategory) ? categoryMap[desireCategory] : null;
                    //Debug.Log(category);

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
                    if (meatfish != Ingredient.MeatFish.noCondition || vege != Ingredient.Vege.noCondition)
                    {
                        if (baseIngred != Ingredient.Base.noCondition || cook != Ingredient.Cook.noCondition)
                        {
                            Debug.Log("01");
                            CustomerManager.instance.GetOrder(personality, true, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
                        }

                        else if (category != null)
                        {
                            Debug.Log("02");
                            CustomerManager.instance.GetOrder(personality, true, meatfish, vege, category, hateMeatFish, hateVege, hateBase);
                        }

                        else
                        {
                            Debug.Log("03");
                            CustomerManager.instance.GetOrder(personality, true, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
                        }
                    }

                    else if (Main != Ingredient.Main.noCondition)
                    {
                        if (baseIngred != Ingredient.Base.noCondition || cook != Ingredient.Cook.noCondition)
                        {
                            Debug.Log("04");
                            CustomerManager.instance.GetOrder(personality, true, Main, baseIngred, cook, hateCategory, hateBase);
                        }
                        else if (category != null)
                        {
                            Debug.Log("05");
                            CustomerManager.instance.GetOrder(personality, true, Main, category, hateCategory, hateBase);
                        }

                        else
                        {
                            Debug.Log("06");
                            CustomerManager.instance.GetOrder(personality, true, Main, baseIngred, cook, hateCategory, hateBase);
                        }
                    }

                    else if (category != null)
                    {
                        Debug.Log("07");
                        CustomerManager.instance.GetOrder(personality, true, Main, category, hateCategory, hateBase);
                    }

                    else
                    {
                        Debug.Log("08");
                        CustomerManager.instance.GetOrder(personality, true, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
                    }

                    skipUI.gameObject.SetActive(false);
                }

                // ���� ���� ui Ȱ��ȭ
                if (currentID.Contains("&&") && !currentID.Contains("R"))
                {
                    string[] chooseTmp = _currentID.Split("&&");

                    multipleskipUI.gameObject.SetActive(true);
                    skipUI.gameObject.SetActive(false);

                    GameObject firstChoose = multipleskipUI.GetChild(0).gameObject.GetComponent<RectTransform>().GetChild(0).gameObject;
                    if (dialogueDic[chooseTmp[0]].line[0] == '@') {
                        firstChoose.GetComponent<TextMeshProUGUI>().text = dialogueDic[chooseTmp[0]].line.Substring(1, dialogueDic[chooseTmp[0]].line.Length - 1);
                    }
                    else
                        firstChoose.GetComponent<TextMeshProUGUI>().text = dialogueDic[chooseTmp[0]].line;
                    GameObject secondChoose = multipleskipUI.GetChild(1).gameObject.GetComponent<RectTransform>().GetChild(0).gameObject;
                    if (dialogueDic[chooseTmp[1]].line[0] == '%')
                    {
                        secondChoose.GetComponent<TextMeshProUGUI>().text = dialogueDic[chooseTmp[1]].line.Substring(1, dialogueDic[chooseTmp[1]].line.Length - 1);
                    }
                    else
                        secondChoose.GetComponent<TextMeshProUGUI>().text = dialogueDic[chooseTmp[1]].line;
                    return;
                }

                if (currentID.Contains("_R_"))
                //if (currentID.Contains("R") && !currentID.Contains("GTR"))
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

        if (_isRandom == false)
        {
            // �÷��� ó���� �ʿ��� ���� �÷��׸� ó��
            if (_currentID.Contains("@@"))
            {
                string[] chooseTmp = _currentID.Split("@@");

                if (GameManager.instance.sneakyAdsFlag == true)
                {
                    _currentID = chooseTmp[0];
                }
                if (GameManager.instance.sneakyAdsFlag == false)
                {
                    _currentID = chooseTmp[1];
                }

            }

            nameUI.text = dialogueDic[currentID].name;
            dialogueUI.text = dialogueDic[currentID].line.Replace('`', ',');
            //Debug.Log(dialogueDic[currentID].line);
        }

        else {

            string currentDialogue = randomDialogues[indexForRandom].line;

            CustomerManager.instance.currentCustomer.InitializeOrder();
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

                        currentDialogue = currentDialogue.Replace("$$", desireMeatfish);

                        if ((desireMeatfish == "����" || desireMeatfish == "������") && randomDialogues[indexForRandom].desireMain[0].Equals('!')) {
                            hateCategory = true;
                        }
                        else if (randomDialogues[indexForRandom].desireMain[0].Equals('!')) {
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

                        currentDialogue = currentDialogue.Replace("$$", desireVege);
                        if ((desireVege == "��ä��") && randomDialogues[indexForRandom].desireMain[0].Equals('!'))
                        {
                            hateCategory = true;
                        }
                        else if (randomDialogues[indexForRandom].desireMain[0].Equals('!'))
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

                    currentDialogue = currentDialogue.Replace("%%", desireVege);
                    if (randomDialogues[indexForRandom].desireMain[0].Equals('!'))
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

                        if (randomDialogues[indexForRandom].desireCategory[0].Equals('!'))
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
                        /*
                        if (randomDialogues[indexForRandom].desireCategory[0].Equals('!'))
                        {
                            hateCategory = true;
                        }
                        */
                    }

                    currentDialogue = currentDialogue.Replace("##", desireCategory);
                    break;

                case "@@":
                case "!@@":
                    desireCategory = withBatchimCategory[UnityEngine.Random.Range(0, withBatchimCategory.Length)];
                    currentDialogue = currentDialogue.Replace("@@", desireCategory);
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
                        currentDialogue = currentDialogue.Replace("**", desireMeatfish);
                        break;

                    case 1:
                        desireVege = noBatchimVege[UnityEngine.Random.Range(0, noBatchimVege.Length)];
                        currentDialogue = currentDialogue.Replace("**", desireVege);
                        break;

                    case 2:
                        desireVege = withBatchimVege[UnityEngine.Random.Range(0, withBatchimVege.Length)];
                        currentDialogue = currentDialogue.Replace("**", desireVege);
                        break;

                    case 3:
                        desireCategory = noBatchimBase[UnityEngine.Random.Range(0, noBatchimBase.Length)];
                        currentDialogue = currentDialogue.Replace("**", desireCategory);
                        break;

                    case 4:
                        desireCategory = noBatchimCook[UnityEngine.Random.Range(0, noBatchimCook.Length)];
                        currentDialogue = currentDialogue.Replace("**", desireCategory);
                        break;

                    case 5:
                        desireCategory = noBatchimCategory[UnityEngine.Random.Range(0, noBatchimCategory.Length)];
                        currentDialogue = currentDialogue.Replace("**", desireCategory);
                        break;

                    case 6:
                        desireCategory = withBatchimCategory[UnityEngine.Random.Range(0, withBatchimCategory.Length)];
                        currentDialogue = currentDialogue.Replace("**", desireCategory);
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

            else if (category != null) {
                Debug.Log(7);
                CustomerManager.instance.GetOrder(personality, false, Main, category, hateCategory, hateBase);
            }

            else
            {
                Debug.Log("����");
                CustomerManager.instance.GetOrder(personality, false, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
            }
            
            Debug.Log(desireMeatfish + " | " + System.Guid.NewGuid());
            Debug.Log(desireVege + " | " + System.Guid.NewGuid());
            Debug.Log(desireCategory + " | " + System.Guid.NewGuid());
            Debug.Log(hateMeatFish + " | " + System.Guid.NewGuid());
            Debug.Log(hateVege + " | " + System.Guid.NewGuid());
            Debug.Log(hateBase + " | " + System.Guid.NewGuid());
            Debug.Log(hateCategory + " | " + System.Guid.NewGuid());
            
            nameUI.text = "�մ�";
            dialogueUI.text = currentDialogue.Replace('`', ',');
            nextUI.gameObject.SetActive(true);
            skipUI.gameObject.SetActive(false);

            Debug.Log("********* " + System.Guid.NewGuid());
            Debug.Log("indexForRandom: " + indexForRandom + " | " + System.Guid.NewGuid());
            Debug.Log("dialogueUI.text: " + dialogueUI.text + " | " + System.Guid.NewGuid());
            Debug.Log("********* " + System.Guid.NewGuid());


            indexForRandom++;
            _Customer++;
            if (_pastID.Split(new char[] { '_' })[1] != "C00")
                GameManager.instance.nextCustomer();


        }
    }

    public void GetNextDialogueMultiple(string index)
    {

        string[] chooseTmp = _currentID.Split("&&");

        if (index == "a")
            _currentID = chooseTmp[0];
        if (index == "b")
            _currentID = chooseTmp[1];

        // �÷��׸� �����ϴ� �������� ���� �÷��׸� ����
        if (dialogueDic[_currentID].line.Contains('@'))
            GameManager.instance.sneakyAdsFlag = true;

        if (dialogueDic[_currentID].line.Contains('%'))
            GameManager.instance.sneakyAdsFlag = false;
        

        multipleskipUI.gameObject.SetActive(false);
        skipUI.gameObject.SetActive(true);

    }
}
