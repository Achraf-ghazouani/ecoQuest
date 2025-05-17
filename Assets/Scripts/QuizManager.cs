using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq; // For shuffling the list
using UnityEngine.SceneManagement; // To reload the scene or change the scene

public class QuizManager : MonoBehaviour
{
    public TMP_Text questionText;
    public Button[] optionButtons;
    public TMP_Text scoreText; // Text to display the score
    private List<Quiz> quizList;
    private Quiz currentQuiz;
    private int score = 0;
    private int questionsAsked = 0;
    private List<Quiz> currentQuizSet;
    public GameObject badge;
public Button closeButton; 
public GameObject quizPanel;
    void Start()
    {
        LoadQuizData();
        GenerateRandomQuestions();
        ShowRandomQuiz();
    }

    // Load the quiz data from the JSON file
    void LoadQuizData()
    {
        TextAsset quizJson = Resources.Load<TextAsset>("quiz_questions");
        QuizList loadedQuizzes = JsonUtility.FromJson<QuizList>("{\"quizzes\":" + quizJson.text + "}");
        quizList = new List<Quiz>(loadedQuizzes.quizzes);
    }

    // Generate a random set of 10 quizzes from the list
    void GenerateRandomQuestions()
    {
        // Shuffle the quiz list and take the first 10 questions
        currentQuizSet = quizList.OrderBy(x => Random.Range(0, 100)).Take(10).ToList();
    }

    // Show a random quiz from the current set
    public void ShowRandomQuiz()
    {
        if (questionsAsked >= currentQuizSet.Count)
        {
            EndGame();
            return;
        }

        currentQuiz = currentQuizSet[questionsAsked];
        questionText.text = currentQuiz.question;
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < currentQuiz.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                TMP_Text btnText = optionButtons[i].GetComponentInChildren<TMP_Text>();
                if (btnText != null)
                    btnText.text = currentQuiz.options[i];

                int index = i; // Capture index for listener
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => CheckAnswer(index));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // Check if the selected answer is correct
    public void CheckAnswer(int selectedIndex)
    {
        if (selectedIndex == currentQuiz.correctAnswerIndex)
        {
            Debug.Log("Correct Answer!");
            score++;  // Increase score on correct answer
        }
        else
        {
            Debug.Log("Wrong Answer!");
        }

        questionsAsked++;
        scoreText.text = "Score: " + score; // Update the score on the UI
        ShowRandomQuiz(); // Show the next question
    }

    // End the game and show the final score
  void EndGame()
{
    Debug.Log("Final Score: " + score);
    scoreText.text = "Final Score: " + score;

    foreach (Button btn in optionButtons)
    {
        btn.interactable = false;
    }

    if (closeButton != null)
    {
        closeButton.gameObject.SetActive(true); // Show the close button
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(CloseQuizPanel);
    }
}
void CloseQuizPanel()
{
    if (quizPanel != null)
        quizPanel.SetActive(false);
        badge.SetActive(true);
}
}
