[System.Serializable]
public class Quiz
{
    public string topic;
    public string question;
    public string[] options;
    public int correctAnswerIndex;
}

[System.Serializable]
public class QuizList
{
    public Quiz[] quizzes;
}
