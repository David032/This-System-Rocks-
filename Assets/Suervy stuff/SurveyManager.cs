using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SurveyManager : MonoBehaviour
{
    public Questions[] questions;
    private static List<Questions> unansweredQuestions;

    private Questions currentQuestion;

    public Text questionText;
    public float timeBetweenQuestions = 1f;

    public Text trueAnswerText;
    public Text falseAnswerText;

    public Animator animator;

    private void Start()
    {
        //load the questions to a list of unaswered questions
        if(unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = questions.ToList<Questions>();

        }

        SetCurrentQuestion();
       // Debug.Log(currentQuestion.fact + " is " + currentQuestion.isTrue);
    }

    void SetCurrentQuestion()
    {
        int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
        currentQuestion = unansweredQuestions[randomQuestionIndex];

        questionText.text = currentQuestion.fact;

        if (currentQuestion.isTrue)
        {
            trueAnswerText.text = "Correct";
            falseAnswerText.text = "Wrong";
        }
        else
        {
            trueAnswerText.text = "Wrong";
            falseAnswerText.text = "Correct";
        }
    }

    public void UserSelectTrue()
    {
        animator.SetTrigger("True");
        if (currentQuestion.isTrue)
        {
            Debug.Log("CORRECT");
        }
        else
        {
            Debug.Log("WRONG");
        }

        StartCoroutine(TransitionToNextQuestion());
    }

    public void UserSelectFalse()
    {
        animator.SetTrigger("False");
        if (!currentQuestion.isTrue)
        {
            Debug.Log("CORRECT");
        }
        else
        {
            Debug.Log("wrong");
        }

        StartCoroutine(TransitionToNextQuestion());
    }

    IEnumerator TransitionToNextQuestion()
    {
        //remove the question after answer
        unansweredQuestions.Remove(currentQuestion);

        yield return new WaitForSeconds(timeBetweenQuestions);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
