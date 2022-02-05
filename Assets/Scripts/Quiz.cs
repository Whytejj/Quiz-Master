using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText; 
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    [SerializeField] QuestionSO currentQuestion; 

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    bool hasAnsweredEarly = true;

    int correctAnswerIndex;

    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    [SerializeField] Sprite wrongAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Progress bar")]
    [SerializeField] Slider progressBar;

    public bool isComplete;



    // Start is called before the first frame update
    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        //DisplayQuestion();

        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Update() {
        timerImage.fillAmount = timer.fillFraction;
        if (timer.loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                return;
            }

            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int index){
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = $"Score: {scoreKeeper.CalculateScore()}%";

    }

    void DisplayAnswer(int index){

        //TODO: fix shit code lol

        string correctAnswer = currentQuestion.GetAnswer(currentQuestion.GetCorrectAnswer());

        if (index == -1)
        {
            Image correctButtonImage = answerButtons[currentQuestion.GetCorrectAnswer()].GetComponent<Image>();

            questionText.text = $"The correct answer: {correctAnswer} ";
            correctButtonImage.sprite = correctAnswerSprite;

        }
        
        else{
            Image buttonImage = answerButtons[index].GetComponent<Image>();


            if(index == currentQuestion.GetCorrectAnswer()){
                questionText.text = "Correct!";
                buttonImage.sprite = correctAnswerSprite;
                scoreKeeper.IncrementCorrectAnswer();
            }

            else{
    
                Image correctButtonImage = answerButtons[currentQuestion.GetCorrectAnswer()].GetComponent<Image>();
                //string correctAnswer = question.GetAnswer(question.GetCorrectAnswer());

                questionText.text = $"Correct answer: {correctAnswer} ";
                buttonImage.sprite = wrongAnswerSprite;

                correctButtonImage.sprite = correctAnswerSprite;
            }
        }
        
    }

    void GetNextQuestion(){

        if (questions.Count > 0 )
        {
            SetButtonState(true);
            SetDefaultSprite();
            GetRandomQuestion();
            DisplayQuestion();
            scoreKeeper.IncrementQuestionSeen();
            progressBar.value++;
        }
    }

    void GetRandomQuestion(){
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];

        if (questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }
        
    }

    void DisplayQuestion(){
        questionText.text = currentQuestion.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = currentQuestion.GetAnswer(i);
        }
    } 

    void SetButtonState(bool state){
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void SetDefaultSprite(){
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }
}
