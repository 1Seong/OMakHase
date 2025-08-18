using NUnit.Framework;
using System;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CookManager : MonoBehaviour
{
    public static CookManager instance;

    public Ingredient.MeatFish meatfish;

    public Ingredient.Vege vege;

    public Ingredient.Base baseIngred;

    public Ingredient.Cook cook;

    public event Action<string> DialogueSetEvent;

    [SerializeField, HideInInspector]
    private int _reputationRise;  // �ν����Ϳ��� ���� �Ұ�

    public int ReputationRise => _reputationRise;


    [SerializeField]
    bool _requestSatisfied;
    public bool requestSatisfied { get => _requestSatisfied; }

    public enum Result { positive, neutral, negative}
    [SerializeField]
    private Result _satisfiedType;
    public Result satisfiedType { get => _satisfiedType; }
    public void setSatisfiedType(Result type) { _satisfiedType = type; }

    private RectTransform _OrderCanvas;

    private RectTransform _NextButton;

    private RectTransform _OrderButton;

    private RectTransform _CookCanvas;

    private RectTransform _DialogueCanvas;

    [SerializeField]
    private RectTransform _SkipButton;

    [SerializeField]
    private GameObject _BaseNextButton;
    [SerializeField]
    private GameObject _CookNextButton;

    public void getMeatFish(int index) 
    {
        meatfish = GetDataFromEnum<Ingredient.MeatFish>(index);
    }

    public void getVege(int index)
    {
        vege = GetDataFromEnum<Ingredient.Vege>(index);
    }

    public void getBase(int index)
    {
        baseIngred = GetDataFromEnum<Ingredient.Base>(index);
        if (!_BaseNextButton.activeSelf)
            _BaseNextButton.SetActive(true);
    }

    public void getCook(int index)
    {
        cook = GetDataFromEnum<Ingredient.Cook>(index);
        if (!_CookNextButton.activeSelf)
            _CookNextButton.SetActive(true);
    }

    public void initCook() { 
        meatfish = Ingredient.MeatFish.noCondition;
        vege = Ingredient.Vege.noCondition;
        baseIngred = Ingredient.Base.noCondition;
        cook = Ingredient.Cook.noCondition;
    }


    T GetDataFromEnum<T>(int index)
    {

        Array enumValues = Enum.GetValues(typeof(T));


        return (T)enumValues.GetValue(index);
    }

    public void cookFood()
    {
        // ���� �丮

        RecipeData food = RecipeManager.instance.GetRecipe(baseIngred, cook, meatfish, vege);
        CustomerData customer = CustomerManager.instance.currentCustomer;

        if (food != null)
        {
            // �� �䱸���� �˻�
            _requestSatisfied = customer.CheckCondition(food);

            //Debug.Log(food.recipeName);
            //Debug.Log(requestSatisfied);

            _reputationRise = judge(food);

            if (GameManager.instance.day != 0) {
                // ���� ����
                if (requestSatisfied)
                {
                    GameManager.instance.reputation += ReputationRise;
                    //CustomerManager.instance.orderText.text = "���ִ�";
                    CustomerManager.instance.orderText.text = DialogueManager.Instance.getRandomReaction(0);
                    setSatisfiedType(Result.positive);
                }
                else
                {

                    if (CustomerManager.instance.currentPersonality == Personality.Picky && food.taste >= 7)
                    {
                        GameManager.instance.reputation += ReputationRise;
                        //CustomerManager.instance.orderText.text = "������ �ϴ�";
                        CustomerManager.instance.orderText.text = DialogueManager.Instance.getRandomReaction(1);
                        setSatisfiedType(Result.neutral);
                    }

                    else if (CustomerManager.instance.currentPersonality == Personality.Normal && food.taste >= 5)
                    {
                        GameManager.instance.reputation += ReputationRise;
                        //CustomerManager.instance.orderText.text = "������ �ϴ�";
                        CustomerManager.instance.orderText.text = DialogueManager.Instance.getRandomReaction(1);
                        setSatisfiedType(Result.neutral);
                    }

                    else if (CustomerManager.instance.currentPersonality == Personality.Generous && food.taste >= 3)
                    {
                        GameManager.instance.reputation += ReputationRise;
                        //CustomerManager.instance.orderText.text = "������ �ϴ�";
                        CustomerManager.instance.orderText.text = DialogueManager.Instance.getRandomReaction(1);
                        setSatisfiedType(Result.neutral);
                    }

                    else
                    {
                        //CustomerManager.instance.orderText.text = "������";
                        CustomerManager.instance.orderText.text = DialogueManager.Instance.getRandomReaction(2);
                        setSatisfiedType(Result.negative);
                    }
                }
            }

            if (DialogueManager.Instance.IsRandom == true) {
                if (satisfiedType == Result.positive)
                {
                    //DialogueManager.Instance.getDialogueUI.text = "���ִ�";
                    var text = DialogueManager.Instance.getRandomReaction(0);
                    DialogueSetEvent!.Invoke(text);
                }
                else if (satisfiedType == Result.neutral)
                {
                    //DialogueManager.Instance.getDialogueUI.text = "������ �ϴ�";
                    var text = DialogueManager.Instance.getRandomReaction(1);
                    DialogueSetEvent!.Invoke(text);
                }
                else if (satisfiedType == Result.negative)
                {
                    //DialogueManager.Instance.getDialogueUI.text = "������";
                    var text = DialogueManager.Instance.getRandomReaction(2);
                    DialogueSetEvent!.Invoke(text);
                }
                else
                    Debug.Log("����");

                _NextButton.gameObject.SetActive(false);
                _SkipButton.gameObject.SetActive(true);
            }
            else
                DialogueManager.Instance.GetNextDialogue();

            Debug.Log("���� ��� ������???");

            _OrderCanvas.gameObject.SetActive(true);

            _CookCanvas.gameObject.SetActive(false);
            //_OrderButton.gameObject.SetActive(true);
            _DialogueCanvas.gameObject.SetActive(true);

            // GTR ���� �ٷ� ������ �� ��, �ֹ� ui ������ �ʵ��� ��
            string tmp1 = DialogueManager.Instance.currentID;
            string tmp2 = DialogueManager.Instance.pastID.Replace('~', '_');
            if (!(!tmp1.Contains("GTR") && tmp1.Contains("O") && tmp2.Contains(tmp1))) {
                if (DialogueManager.Instance.IsRandom != true)
                {

                    _NextButton.gameObject.SetActive(false);
                    _SkipButton.gameObject.SetActive(true);
                }
            }

            initCook();
        }
        else 
        {
            Debug.Log("���� �������Դϴ�");
        }

    }

    public int judge(RecipeData food ) 
    {
        int person = 0;
        if (CustomerManager.instance.currentPersonality == Personality.Picky)
        {
            person = 5;
        }

        else if (CustomerManager.instance.currentPersonality == Personality.Normal)
        {
            person = 5;
        }

        else if (CustomerManager.instance.currentPersonality == Personality.Generous)
        {
            person = 3;
        }

        else
        {
            person = 7;
        }

        if (food.isNew) {
            return 6 + Math.Max((food.taste - person), 0);
        }

        else
        {
            return 3 + Math.Max((food.taste - person), 0);
        }

    }

    private void Awake()
    {
        instance = this;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        _OrderCanvas = GameManager.instance.Order_Canvas.GetComponent<RectTransform>();
        //Debug.Log(_OrderCanvas);
        _NextButton = GameManager.instance.Order_Canvas.transform.GetChild(1).GetComponent<RectTransform>();
        //Debug.Log(_NextButton);
        _OrderButton = GameManager.instance.Order_Canvas.transform.GetChild(2).GetComponent<RectTransform>();
        //Debug.Log(_OrderButton);
        _CookCanvas = GameManager.instance.Cook_Canvas.GetComponent<RectTransform>();
        //Debug.Log(_OrderCanvas);

        _DialogueCanvas = GameManager.instance.Dialogue_Canvas.GetChild(0).GetComponent<RectTransform>();
        //_SkipButton = GameManager.instance.Dialogue_Canvas.GetChild(0).GetChild(3).GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
