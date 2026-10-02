namespace Codeigo.Models;

// ── Exercise types ───────────────────────────────────────────────────────────

public enum ExerciseType
{
    MultipleChoice,
    FillBlank,
    ArrangeCode,
    PredictOutput,
    TypeItOut
}

// ── Core exercise (shared schema for lessons AND custom quizzes) ─────────────

public class Exercise
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public ExerciseType Type { get; set; }
    public string Prompt { get; set; } = "";

    // Optional code snippet shown to the learner
    public string? Code { get; set; }

    // Fill-blank: tokens inside [[ ]] become blanks
    // Multiple choice: Options list, CorrectIndex
    // Arrange: code is split into tokens, shuffled
    // Predict: ExpectedOutput
    // TypeItOut: TargetLine

    public List<string> Options { get; set; } = [];   // multiple choice options
    public int CorrectIndex { get; set; }              // multiple choice answer index

    public List<string> AcceptedAnswers { get; set; } = []; // fill-blank / typeout
    public string? ExpectedOutput { get; set; }             // predict
    public string? TargetLine { get; set; }                 // type-it-out

    // Tokens for arrange-code (if null, derived from Code at runtime)
    public List<string>? Tokens { get; set; }
    public List<string>? DistractorTokens { get; set; }

    public string? Explanation { get; set; }

    // Metadata (used by search / progress)
    public string Language { get; set; } = "python";
    public string Track { get; set; } = "";
    public string Skill { get; set; } = "";
    public int Difficulty { get; set; } = 1; // 1-3
}

// ── Lesson ───────────────────────────────────────────────────────────────────

public class Lesson
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public List<Exercise> Exercises { get; set; } = [];
}

// ── Track (a group of ordered lessons) ──────────────────────────────────────

public class Track
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public List<Lesson> Lessons { get; set; } = [];
}

// ── Language curriculum ──────────────────────────────────────────────────────

public class LanguageCurriculum
{
    public string Id { get; set; } = "";        // "python" | "javascript" | "cpp"
    public string DisplayName { get; set; } = "";
    public string Emoji { get; set; } = "";
    public string AccentClass { get; set; } = ""; // CSS class: "py" | "js" | "cpp"
    public List<Track> Tracks { get; set; } = [];
}

// ── Custom quiz ──────────────────────────────────────────────────────────────

public enum QuizVisibility { Private, Unlisted }

public class CustomQuiz
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Language { get; set; } = "python";
    public QuizVisibility Visibility { get; set; } = QuizVisibility.Private;
    public string AuthorId { get; set; } = "guest";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Exercise> Exercises { get; set; } = [];

    public string ShareCode => Id.ToUpper()[..4] + "-" + Id[4..];
}

// ── Progress tracking ────────────────────────────────────────────────────────

public class UserProgress
{
    public int Xp { get; set; }
    public int Streak { get; set; }
    public int DailyXp { get; set; }
    public int DailyGoal { get; set; } = 30;
    public HashSet<string> CompletedLessonIds { get; set; } = [];
    public Dictionary<string, int> LessonAccuracy { get; set; } = [];
}

// ── Lesson session result ────────────────────────────────────────────────────

public class LessonResult
{
    public string LessonId { get; set; } = "";
    public int TotalExercises { get; set; }
    public int CorrectAnswers { get; set; }
    public int XpEarned { get; set; }
    public TimeSpan Duration { get; set; }

    public int AccuracyPercent =>
        TotalExercises == 0 ? 0 : (int)Math.Round(CorrectAnswers * 100.0 / TotalExercises);
}

// ── Quests & Challenges ──────────────────────────────────────────────────────

public class DailyQuest
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "🎯";
    public int Target { get; set; }
    public int XpReward { get; set; }
    public bool IsClaimed { get; set; }
}

// ── Leaderboard Entry ────────────────────────────────────────────────────────

public class LeaderboardEntry
{
    public int Rank { get; set; }
    public string UserName { get; set; } = "";
    public string AvatarEmoji { get; set; } = "🐱";
    public string MascotColor { get; set; } = "var(--p)";
    public int Xp { get; set; }
    public int Streak { get; set; }
    public string PrimaryLanguage { get; set; } = "Python";
    public bool IsCurrentUser { get; set; }
}

