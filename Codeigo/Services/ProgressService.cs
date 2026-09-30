using Codeigo.Models;

namespace Codeigo.Services;

public class ProgressService
{
    public event Action? OnChange;

    public int Xp { get; private set; } = 340;
    public int Streak { get; private set; } = 12;
    public int DailyXp { get; private set; } = 20;
    public int DailyGoal { get; set; } = 30;
    public int Hearts { get; private set; } = 5;
    public bool HeartsEnabled { get; set; } = false; // Off by default as decided

    public string CurrentLanguage { get; set; } = "python";

    // User authentication state
    public bool IsLoggedIn { get; private set; } = false;
    public string UserName { get; private set; } = "Guest";

    private readonly HashSet<string> _completedLessons = ["py-basics-1", "py-basics-2"];
    private readonly Dictionary<string, LessonResult> _recentResults = [];

    public bool IsLessonCompleted(string lessonId) => _completedLessons.Contains(lessonId);

    public void SetLanguage(string langId)
    {
        CurrentLanguage = langId;
        NotifyStateChanged();
    }

    public void Login(string name = "Ethan")
    {
        IsLoggedIn = true;
        UserName = name;
        NotifyStateChanged();
    }

    public void Logout()
    {
        IsLoggedIn = false;
        UserName = "Guest";
        NotifyStateChanged();
    }

    public void RecordLessonComplete(string lessonId, int correct, int total, TimeSpan duration)
    {
        int earnedXp = Math.Max(10, correct * 10);
        Xp += earnedXp;
        DailyXp += earnedXp;

        if (!_completedLessons.Contains(lessonId))
        {
            _completedLessons.Add(lessonId);
        }

        var result = new LessonResult
        {
            LessonId = lessonId,
            CorrectAnswers = correct,
            TotalExercises = total,
            XpEarned = earnedXp,
            Duration = duration
        };

        _recentResults[lessonId] = result;
        NotifyStateChanged();
    }

    public LessonResult? GetLastResult(string lessonId)
    {
        return _recentResults.TryGetValue(lessonId, out var res) ? res : null;
    }

    public void LoseHeart()
    {
        if (HeartsEnabled && Hearts > 0)
        {
            Hearts--;
            NotifyStateChanged();
        }
    }

    public void ResetHearts()
    {
        Hearts = 5;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
