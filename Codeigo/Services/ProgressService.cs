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

    // ── Quests Management ────────────────────────────────────────────────────
    private readonly HashSet<string> _claimedQuests = [];

    public bool IsQuestClaimed(string questId) => _claimedQuests.Contains(questId);

    public int GetQuestProgress(string questId)
    {
        return questId switch
        {
            "q-daily-xp" => DailyXp,
            "q-lessons" => _completedLessons.Count,
            "q-streak" => Streak,
            "q-polyglot" => 1,
            _ => 0
        };
    }

    public List<DailyQuest> GetDailyQuests() =>
    [
        new DailyQuest
        {
            Id = "q-daily-xp",
            Title = "Daily Dedication",
            Description = "Earn 30 XP today through drills or quizzes",
            Icon = "⚡",
            Target = 30,
            XpReward = 20,
            IsClaimed = IsQuestClaimed("q-daily-xp")
        },
        new DailyQuest
        {
            Id = "q-lessons",
            Title = "Syntax Sprinter",
            Description = "Complete 2 lessons or practice exercises",
            Icon = "🚀",
            Target = 2,
            XpReward = 25,
            IsClaimed = IsQuestClaimed("q-lessons")
        },
        new DailyQuest
        {
            Id = "q-streak",
            Title = "Streak Master",
            Description = "Maintain a 10-day practice streak",
            Icon = "🔥",
            Target = 10,
            XpReward = 30,
            IsClaimed = IsQuestClaimed("q-streak")
        },
        new DailyQuest
        {
            Id = "q-polyglot",
            Title = "Polyglot Explorer",
            Description = "Explore lessons in multiple programming languages",
            Icon = "🌐",
            Target = 1,
            XpReward = 15,
            IsClaimed = IsQuestClaimed("q-polyglot")
        }
    ];

    public bool ClaimQuest(string questId)
    {
        if (_claimedQuests.Contains(questId)) return false;
        var quests = GetDailyQuests();
        var q = quests.FirstOrDefault(x => x.Id == questId);
        if (q == null) return false;

        var prog = GetQuestProgress(questId);
        if (prog >= q.Target)
        {
            _claimedQuests.Add(questId);
            Xp += q.XpReward;
            DailyXp += q.XpReward;
            NotifyStateChanged();
            return true;
        }
        return false;
    }

    // ── Leaderboard Management ───────────────────────────────────────────────
    public List<LeaderboardEntry> GetLeaderboard()
    {
        var peers = new List<LeaderboardEntry>
        {
            new() { UserName = "ByteNinja", AvatarEmoji = "🦊", MascotColor = "#3BB8F5", Xp = 480, Streak = 19, PrimaryLanguage = "Python" },
            new() { UserName = "SarahCode", AvatarEmoji = "🦉", MascotColor = "#22C997", Xp = 420, Streak = 14, PrimaryLanguage = "JavaScript" },
            new() { UserName = "DevAlex", AvatarEmoji = "🐯", MascotColor = "#FFC933", Xp = 360, Streak = 11, PrimaryLanguage = "C++" },
            new() { UserName = "CodeigoBot", AvatarEmoji = "🤖", MascotColor = "#5B4BFF", Xp = 310, Streak = 30, PrimaryLanguage = "Python" },
            new() { UserName = "PixelWhiz", AvatarEmoji = "🐼", MascotColor = "#FF5D6C", Xp = 270, Streak = 8, PrimaryLanguage = "JavaScript" },
            new() { UserName = "SyntaxSam", AvatarEmoji = "🦁", MascotColor = "#22C997", Xp = 230, Streak = 5, PrimaryLanguage = "Python" },
            new() { UserName = "LambdaLuna", AvatarEmoji = "🐰", MascotColor = "#3BB8F5", Xp = 190, Streak = 4, PrimaryLanguage = "C++" },
            new() { UserName = "C_Plus_Plus_Chad", AvatarEmoji = "🐲", MascotColor = "#FF5D6C", Xp = 140, Streak = 3, PrimaryLanguage = "C++" },
            new() { UserName = "ScriptKiddie", AvatarEmoji = "🐹", MascotColor = "#FFC933", Xp = 90, Streak = 2, PrimaryLanguage = "JavaScript" }
        };

        // Add current user
        peers.Add(new LeaderboardEntry
        {
            UserName = UserName,
            AvatarEmoji = "👾",
            MascotColor = "#5B4BFF",
            Xp = Xp,
            Streak = Streak,
            PrimaryLanguage = CurrentLanguage == "javascript" ? "JavaScript" : CurrentLanguage == "cpp" ? "C++" : "Python",
            IsCurrentUser = true
        });

        var sorted = peers.OrderByDescending(p => p.Xp).ToList();
        for (int i = 0; i < sorted.Count; i++)
        {
            sorted[i].Rank = i + 1;
        }

        return sorted;
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
