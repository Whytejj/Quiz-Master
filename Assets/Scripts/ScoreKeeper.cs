using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    int correctAnswer = 0;
    int questionSeen = 0;
    // Start is called before the first frame update
    
    public int GetCorrectAnswer(){
        return correctAnswer;
    }

    public int GetQuestionScene(){
        return questionSeen;
    }

    public void IncrementQuestionSeen(){
        questionSeen++;
    }

    public void IncrementCorrectAnswer(){
        correctAnswer++;
    }

    public int CalculateScore(){
        return Mathf.RoundToInt(correctAnswer / (float) questionSeen * 100);
    }
}
