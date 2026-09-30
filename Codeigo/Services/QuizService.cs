using Codeigo.Models;

namespace Codeigo.Services;

public class QuizService
{
    private readonly List<CustomQuiz> _quizzes = [];

    public QuizService()
    {
        SeedDefaultQuizzes();
    }

    public IReadOnlyList<CustomQuiz> GetQuizzes() => _quizzes.OrderByDescending(q => q.CreatedAt).ToList();

    public CustomQuiz? GetQuizById(string id) =>
        _quizzes.FirstOrDefault(q => q.Id.Equals(id, StringComparison.OrdinalIgnoreCase) ||
                                     q.ShareCode.Equals(id, StringComparison.OrdinalIgnoreCase));

    public void SaveQuiz(CustomQuiz quiz)
    {
        var existing = _quizzes.FirstOrDefault(q => q.Id == quiz.Id);
        if (existing != null)
        {
            var index = _quizzes.IndexOf(existing);
            _quizzes[index] = quiz;
        }
        else
        {
            _quizzes.Add(quiz);
        }
    }

    public bool DeleteQuiz(string id)
    {
        var item = _quizzes.FirstOrDefault(q => q.Id == id);
        if (item != null)
        {
            return _quizzes.Remove(item);
        }
        return false;
    }

    private void SeedDefaultQuizzes()
    {
        _quizzes.Add(new CustomQuiz
        {
            Id = "quiz-py-slice",
            Title = "List slicing warm-up",
            Description = "Master negative indices and slice bounds in Python.",
            Language = "python",
            Visibility = QuizVisibility.Unlisted,
            AuthorId = "Ethan",
            CreatedAt = DateTime.UtcNow.AddDays(-2),
            Exercises =
            [
                new Exercise
                {
                    Type = ExerciseType.FillBlank,
                    Prompt = "Finish the loop so it runs 5 times",
                    Code = "for i in [[range]](5):\n    print(i)",
                    AcceptedAnswers = ["range"],
                    Explanation = "range(5) counts from 0 to 4.",
                    Language = "python"
                },
                new Exercise
                {
                    Type = ExerciseType.MultipleChoice,
                    Prompt = "What does nums[1:3] return for nums = [10, 20, 30, 40]?",
                    Options = ["[10, 20]", "[20, 30]", "[20, 30, 40]", "[10, 20, 30]"],
                    CorrectIndex = 1,
                    Explanation = "Slicing [1:3] includes index 1 up to (but not including) index 3.",
                    Language = "python"
                },
                new Exercise
                {
                    Type = ExerciseType.PredictOutput,
                    Prompt = "What is the output?",
                    Code = "nums = [10, 20, 30]\nprint(nums[-1])",
                    ExpectedOutput = "30",
                    Explanation = "Negative index -1 returns the last item in a list.",
                    Language = "python"
                }
            ]
        });

        _quizzes.Add(new CustomQuiz
        {
            Id = "quiz-js-methods",
            Title = "Array methods",
            Description = "Essential map, filter, and reduce operations in modern JS.",
            Language = "javascript",
            Visibility = QuizVisibility.Private,
            AuthorId = "Ethan",
            CreatedAt = DateTime.UtcNow.AddDays(-4),
            Exercises =
            [
                new Exercise
                {
                    Type = ExerciseType.MultipleChoice,
                    Prompt = "Which method creates a new array with all elements that pass a test?",
                    Options = ["forEach()", "filter()", "map()", "some()"],
                    CorrectIndex = 1,
                    Explanation = "filter() returns a new array with only truthy elements from the callback predicate.",
                    Language = "javascript"
                },
                new Exercise
                {
                    Type = ExerciseType.FillBlank,
                    Prompt = "Transform numbers by multiplying by 2",
                    Code = "const doubled = nums.[[map]](x => x * 2);",
                    AcceptedAnswers = ["map"],
                    Explanation = "map() transforms each element using a callback function.",
                    Language = "javascript"
                }
            ]
        });

        _quizzes.Add(new CustomQuiz
        {
            Id = "quiz-cpp-ptr",
            Title = "Pointer basics",
            Description = "Pointers, addresses, and dereferencing in C++.",
            Language = "cpp",
            Visibility = QuizVisibility.Private,
            AuthorId = "Ethan",
            CreatedAt = DateTime.UtcNow.AddDays(-6),
            Exercises =
            [
                new Exercise
                {
                    Type = ExerciseType.MultipleChoice,
                    Prompt = "Which operator gets the memory address of a variable?",
                    Options = ["*", "&", "->", "%"],
                    CorrectIndex = 1,
                    Explanation = "& is the address-of operator.",
                    Language = "cpp"
                },
                new Exercise
                {
                    Type = ExerciseType.FillBlank,
                    Prompt = "Dereference pointer ptr to read value",
                    Code = "int val = [[*]]ptr;",
                    AcceptedAnswers = ["*"],
                    Explanation = "*ptr dereferences the pointer to access the value at the address.",
                    Language = "cpp"
                }
            ]
        });
    }
}
